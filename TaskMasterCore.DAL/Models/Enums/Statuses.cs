using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TaskMasterCore.DAL.Models.Enums
{
    /// <summary>
    /// Статусы задач
    /// </summary>
    public enum Statuses
    {
        /// <summary>
        /// Назначена
        /// </summary>
        [Display(Name = "Назначена")]
        Assigned = 0,
        /// <summary>
        /// Выполняется
        /// </summary>
        [Display(Name = "Выполняется")]
        InProgress = 1,
        /// <summary>
        /// Приостановлена
        /// </summary>
        [Display(Name = "Приостановлена")]
        Paused = 2,
        /// <summary>
        /// Завершена
        /// </summary>
        [Display(Name = "Завершена")]
        Completed = 3
    }
}
