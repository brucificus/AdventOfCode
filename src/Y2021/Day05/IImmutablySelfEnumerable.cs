public interface IImmutablySelfEnumerable<TSelf>
    where TSelf : IImmutablySelfEnumerable<TSelf>
{
    TSelf Reset();
    OneOf<TSelf, Boundary> MoveNext();
}
