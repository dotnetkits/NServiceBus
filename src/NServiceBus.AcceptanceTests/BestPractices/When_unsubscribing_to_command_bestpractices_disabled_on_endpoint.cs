﻿namespace NServiceBus.AcceptanceTests.BestPractices
{
    using System.Threading.Tasks;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NServiceBus.Features;
    using NUnit.Framework;

    public class When_unsubscribing_to_command_bestpractices_disabled_on_endpoint : NServiceBusAcceptanceTest
    {
       [Test]
        public async Task Should_allow_unsubscribing_to_commands()
        {
            await Scenario.Define<Context>()
                .WithEndpoint<Endpoint>(b => b.Given((bus, c) =>
                {
                    bus.Unsubscribe<MyCommand>();
                    return Task.FromResult(0);
                }))
                .Done(c => c.EndpointsStarted)
                .Run();
        }

        public class Context : ScenarioContext
        {
        }

        public class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>(c => c.DisableFeature<BestPracticeEnforcement>())
                    .AddMapping<MyCommand>(typeof(Endpoint))
                    .AddMapping<MyEvent>(typeof(Endpoint));
            }

            public class Handler : IHandleMessages<MyEvent>
            {
                public void Handle(MyEvent message)
                {
                }
            }
        }
        public class MyCommand : ICommand { }
        public class MyEvent : IEvent { }
    }
}