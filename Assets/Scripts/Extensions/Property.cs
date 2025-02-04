using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property<T>
{
    private Func<T> _getter;
    private Action<T> _setter;
    public Property(Func<T> getter, Action<T> setter)
    {
        _setter = setter ?? throw new ArgumentNullException(nameof(setter));
        _getter = getter ?? throw new ArgumentNullException(nameof(getter));
    }
    public T Value
    {
        get { return _getter(); }
        set { _setter(value); }
    }
}
