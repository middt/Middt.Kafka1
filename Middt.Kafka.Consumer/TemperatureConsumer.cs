using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middt.Kafka.Common;


namespace Middt.Kafka.Consumer
{
    public class TemperatureConsumer: BaseConsumer<TemperatureModel>
    {
        public TemperatureConsumer(KafkaSettings kafkaSettings) : base(kafkaSettings)
        {

        }
        public override  async Task Process(TemperatureModel model)
        {

// Go to black

        }
    }
}
