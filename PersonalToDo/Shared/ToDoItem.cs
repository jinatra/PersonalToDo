using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalToDo.Shared
{
    public class ToDoItem : CosmosModelBase
    {
        public string Title { get; set; } = String.Empty;
        public bool IsDone { get; set; } = false;
        public override string ClassType => "ToDoItem";
    }
}
