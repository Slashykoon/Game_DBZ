using System;
using System.Collections;

class StackSequence: Stack
{
    public delegate void ItemAddedDelegate(object item);
    public delegate object ItemPoppedDelegate();
    public event ItemAddedDelegate ItemAdded;
    public event ItemPoppedDelegate ItemPopped;

    public override void Push(object obj)
    {
        base.Push(obj);
        if (ItemAdded != null)
        {
            ItemAdded(obj);
        }
    }

    public override object Pop()
    {
        object tmp_popped;
        tmp_popped = base.Pop();
        ItemPopped();
        return tmp_popped;
    }

}