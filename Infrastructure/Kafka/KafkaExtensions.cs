using System;
using System.Linq;

using Confluent.Kafka;

using Kafka.Base;
using Kafka.Base.Interfaces;
using Kafka.Factories;
using Kafka.Factories.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kafka
{
	public static class KafkaExtensions
	{

		public static IServiceCollection AddKafkaConsumer(this IServiceCollection services,
			string name,
			Func<IServiceProvider, ConsumerConfig> consumerConfigFactory)
		{
			return services.AddKafkaFactory()
				.AddTransient<IBaseConsumer>(sp =>
				{
					ConsumerConfig options = consumerConfigFactory(sp);
					ILogger<BaseConsumer> logger = sp.GetRequiredService<ILogger<BaseConsumer>>();

					return new BaseConsumer(name, options, logger);
				});

		}

		public static IServiceCollection AddKafkaProducer(this IServiceCollection services,
														 string name,
														 Func<IServiceProvider, ProducerConfig> producerConfigFactory)
		{
			return services.AddKafkaFactory()
				.AddTransient<IBaseProducer>(sp =>
				{
					ProducerConfig options = producerConfigFactory(sp);
					ILogger<BaseProducer> logger = sp.GetRequiredService<ILogger<BaseProducer>>();

					return new BaseProducer(name, options, logger);
				});
		}


		public static IServiceCollection AddKafkaFactory(this IServiceCollection services)
		{
			if (!services.Any(s => s.ServiceType == typeof(IKafkaFactory)))
			{
				services.AddTransient<IKafkaFactory, KafkaFactory>();
			}

			return services;
		}
	}
}