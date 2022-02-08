using System;
using Middt.Kafka.Common;
using Middt.Kafka.Consumer;

KafkaSettings kafkaSettings = new()
{
    BootstrapServers = "localhost:9092",
    GroupId = "group1",
    Topic = "topic-test",
  //  SslCaLocation = "nonprod-kafka-ca-certs.crt"
};

var consumer = new TemperatureConsumer(kafkaSettings);
consumer.OnConsume += ((TemperatureModel model) =>
{
    Console.WriteLine($"Data Received - {model.ToString()}");
});
await consumer.ConsumeAsync();
