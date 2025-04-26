using System;
using System.Threading;
using System.Threading.Tasks;

using Confluent.Kafka;

using Kafka.Base.Configurations;
using Kafka.Base.Interfaces;
using Kafka.Base.Models;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace Kafka.Base
{
	public abstract class KafkaScheduledService : BackgroundService
	{
		protected ILogger<KafkaScheduledService> _logger;
		protected readonly IBaseConsumer _serviceConsumer;
		protected readonly IBaseProducer _serviceProducer;
		private readonly KafkaScheduledServiceConfiguration _configuration;

		private readonly TimeOnly _start;
		private readonly TimeOnly _end;
		private readonly bool _isNightShift;

		public KafkaScheduledService(IOptions<KafkaScheduledServiceConfiguration> configuration,
			IKafkaFactory kafkaFactory,
			ILogger<KafkaScheduledService> logger)
		{
			_configuration = configuration.Value;

			_start = TimeOnly.ParseExact(_configuration.StartTime, "HH:mm");
			_end = TimeOnly.ParseExact(_configuration.EndTime, "HH:mm");
			_isNightShift = _start > _end;

			_serviceConsumer = kafkaFactory.GetConsumer("ServiceConsumer");
			_serviceProducer = kafkaFactory.GetProducer("ServiceProducer");

			_logger = logger;
		}

		protected abstract Task ExecuteInternalAsync(string consumedMessage, CancellationToken stoppingToken);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await Task.Yield();

			_serviceConsumer.Subscribe(_configuration.SourceTopic);
			ConsumeResult<Ignore, string> consumeResult = null;

			while (!stoppingToken.IsCancellationRequested)
			{
				if (IsOutOfShift(TimeOnly.FromDateTime(DateTime.Now), out TimeSpan delay))
				{
					await Task.Delay(delay, stoppingToken);
				}

				try
				{
					_logger.LogInformation("================================================================================");

					consumeResult = await _serviceConsumer.ConsumeWithDelayAsync(TimeSpan.FromSeconds(_configuration.Delay), stoppingToken);
					_logger.LogInformation("Consumed message {message}", consumeResult.Message.Value);

					await ExecuteInternalAsync(consumeResult.Message.Value, stoppingToken);
				}
				catch (Exception e)
				{
					string data = consumeResult is null && consumeResult.Message is null ? "" : consumeResult.Message.Value;

					_logger.LogError("{errorMessage}", e.Message);
					_logger.LogError("Record: {data}", data);
					_logger.LogError("{stackTrace}", e.StackTrace);

					ErrorData errorData = new()
					{
						ServiceName = GetType().Assembly.GetName().Name,
						ErrorMessage = e.Message,
						StackTrace = e.StackTrace,
						Data = data
					};

					_serviceProducer.SendFlush(_configuration.ErrorTopic, JsonConvert.SerializeObject(errorData), TimeSpan.FromSeconds(10));
				}
			}
		}

		private bool IsOutOfShift(TimeOnly time, out TimeSpan delay)
		{
			if (_isNightShift && _end < time && time < _start ||
				!_isNightShift && (_end < time || time < _start))
			{
				bool nextShiftIsToday = _isNightShift ? true : time < _start;

				DateTime nextStart = DateTime.Now.Date
							.AddDays(nextShiftIsToday ? 0 : 1)
							.AddHours(_start.Hour)
							.AddMinutes(_start.Minute);
				delay = nextStart - DateTime.Now;

				_logger.LogInformation("Execution is out of shift ({start}-{end}). Next execution time: {nextStart:HH:mm:ss dd.MM.yyyy}",
										_start,
										_end,
										nextStart.ToString("HH:mm:ss dd.MM.yyyy"));
				return true;
			}

			delay = TimeSpan.Zero;
			return false;
		}
	}
}