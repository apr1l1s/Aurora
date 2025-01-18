using MediatR;

namespace Core.Providers.Cqrs;

public interface IUseCaseDispatcher
{
    Task<TOut> DispatchAsync<TOut>(IUseCase<TOut> useCase, CancellationToken cancellationToken);

    Task DispatchAsync(IUseCase useCase, CancellationToken cancellationToken);
}

public interface IUseCaseHandler<in TIn, TOut>
    : IRequestHandler<TIn, TOut>
    where TIn : IUseCase<TOut>;

public interface IUseCaseHandler<in TIn>
    : IRequestHandler<TIn, EmptyUseCaseOutputDto>
    where TIn : IUseCase
{
    async Task<EmptyUseCaseOutputDto> IRequestHandler<TIn, EmptyUseCaseOutputDto>.Handle(TIn request,
        CancellationToken cancellationToken)
    {
        await Handle(request, cancellationToken).ConfigureAwait(false);

        return EmptyUseCaseOutputDto.Value;
    }

    new Task Handle(TIn request, CancellationToken cancellationToken);
}

public interface IUseCase
    : IUseCase<EmptyUseCaseOutputDto>;

public interface IUseCase<out TOut>
    : IRequest<TOut>;

public class EmptyUseCaseOutputDto
{
    public static readonly EmptyUseCaseOutputDto Value = new();
}