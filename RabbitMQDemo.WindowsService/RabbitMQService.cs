using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQDemo.WindowsService
{
    partial class RabbitMQService : ServiceBase
    {
        private readonly RabbitMQManager rabbitMQManager;
        public RabbitMQService()
        {
            InitializeComponent();
            this.rabbitMQManager = new RabbitMQManager();
        }

        protected override void OnStart(string[] args)
        {
            this.rabbitMQManager.Setup();
        }

        protected override void OnStop()
        {
            this.rabbitMQManager.Release();
        }

        protected override void OnShutdown()
        {
            this.rabbitMQManager.Release();
        }
    }
}
