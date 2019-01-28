[![Lykke](https://avatars3.githubusercontent.com/u/14153330?v=3&s=200)](https://avatars3.githubusercontent.com/u/14153330?v=3&s=200) 
# Lykke.Cqrs.MessageCancellation

Lykke.Cqrs.MessageCancellation is a plugin for Lykke services that use Lykke.Cqrs libriary. It consists of:

  - Asp.Net Core Controller 
  - Command/Events Interceptors For Lykke.Cqrs
  - Internal Services

# How to use

  - Install nuget Lykke.Cqrs.MessageCancellation
  - Create CustomMessageController.cs in Controllers folder and inherit it from Lykke.Cqrs.MessageCancellation.Controllers.MessageCancellationController. The endpoint is "/api/plugin/message-cancellation"
  - Register Cqrs Message Cancellation
  
```sh
    var builder = new ContainerBuilder();
    builder.RegisterCqrsMessageCancellation((options) =>
    {
        options.Value
            .MapMessageId<MessageWithSomeId>(x => x.MessageId)
            .MapMessageId<MessageWithGuidId>(x => x.OperationId.ToString());
    }); 
```

   - Add Command/Event interceptors in cqrs pipeline

```sh
    protected virtual IRegistration[] GetInterceptors()
    {
        return new IRegistration[]
        {
            Register.CommandInterceptor<MessageCancellationCommandInterceptor>(),
            Register.EventInterceptor<MessageCancellationEventInterceptor>()
        };
    }
```
```sh
    var interceptors = GetInterceptors();
    if (interceptors != null)
    {
        registration.AddRange(interceptors);
    }

    var cqrsEngine = new CqrsEngine(
        logFactory,
        new AutofacDependencyResolver(ctx.Resolve<IComponentContext>()),
        messagingEngine,
        new DefaultEndpointProvider(),
        true,
        registration.ToArray());
```

