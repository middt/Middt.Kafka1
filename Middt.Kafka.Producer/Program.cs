using System.Net;
using Middt.Kafka.Common;

KafkaSettings kafkaSettings = new()
{
    BootstrapServers = "localhost:9092",
    GroupId = "group1",
    Topic = "topic-test",
  //  SslCaLocation = "nonprod-kafka-ca-certs.crt"
};

var producer = new BaseProducer<TemperatureModel>(kafkaSettings);
TemperatureModel model;

int startIndex = 0;
int endIndex = 0;
while (true)
{
    endIndex = startIndex + 100;

    for (int i = startIndex; i < endIndex; i++)
    {
        model = Generate(i);
        await producer.ProduceAsync(model);

        await Task.Delay(1000);
    }

    Console.WriteLine("Publish Success!");

    startIndex = endIndex;
}


TemperatureModel Generate(int index)
{
    TemperatureModel model = new TemperatureModel();
    model.Index = index;
    model.HighTemp = new Random().Next(40, 50);
    model.LowTemp = new Random().Next(10, 20);
    model.Mean = (model.HighTemp + model.LowTemp) / 2;

    return model;
}
//IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
//{
//    for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
//        yield return day;
//}

Console.ReadLine();
