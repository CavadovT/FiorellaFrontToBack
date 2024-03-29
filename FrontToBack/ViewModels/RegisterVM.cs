﻿using System.ComponentModel.DataAnnotations;

namespace FrontToBack.ViewModels
{
    public class RegisterVM
    {
        [Required,StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(100)]

        public string Username { get; set; }

        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,DataType(DataType.Password),Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}
