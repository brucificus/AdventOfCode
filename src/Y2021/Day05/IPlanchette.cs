public interface IPlanchette<out TCellContent, out TParent>
{
    TCellContent Peek();

    TParent Parent { get; }
}
