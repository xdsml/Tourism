using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu;

public static class ViewExtensions
{
    public static T AssignToGrid<T>(this T view, int row, int column) where T : View
    {
        Grid.SetRow(view, row);
        Grid.SetColumn(view, column);
        return view;
    }
}
