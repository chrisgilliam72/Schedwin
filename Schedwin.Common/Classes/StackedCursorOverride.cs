using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Common
{
    public class StackedCursorOverride : IDisposable
    {
        private readonly static Stack<Cursor> CursorStack;

        static StackedCursorOverride()
        {
            CursorStack = new Stack<Cursor>();
        }

        public StackedCursorOverride(Cursor cursor)
        {
            CursorStack.Push(cursor);
            Mouse.OverrideCursor = cursor;
        }

        public void Dispose()
        {
            var previousCursor = CursorStack.Pop();
            if (CursorStack.Count == 0)
            {
                Mouse.OverrideCursor = null;
                return;
            }

            // if next cursor is the same as the one we just popped, don't change the override
            if ((CursorStack.Count > 0) && (CursorStack.Peek() != previousCursor))
                Mouse.OverrideCursor = CursorStack.Peek();
        }
    }

}
