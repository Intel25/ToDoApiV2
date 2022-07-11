using System;

namespace ToDoApiV2.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DeadLine { get; set; }
    }
}
