using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMVVM.Model;
using UnityMVVM.View;

public class ToDoItem : ModelBase
{
    public string Label { get; set; }

    public bool IsComplete { get; set; }
}

public class ToDoItemView : CollectionViewItemBase<ToDoItem>
{
    public override void InitItem(ToDoItem model, int idx)
    {

    }

    public override void UpdateItem(ToDoItem model, int newIdx)
    {

    }
}
