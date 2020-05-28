﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TestGenerator.Model.Entities
{
    public class Module
    {
        private ICollection<Question> _questions;
        private ICollection<Exam> _exams;

        public Module()
        {
        }

        private Module(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        private ILazyLoader LazyLoader { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name="Titre")]
        public string Title { get; set; }

        [Display(Name="Description du module")]
        public string Description { get; set; }

        [Display(Name="Questions liées au module")]
        public ICollection<Question> Questions { 
            get => LazyLoader.Load(this, ref _questions); 
            set => _questions = value; 
        }

        [Display(Name="Examens liés au module")]
        public ICollection<Exam> Exams
        {
            get => LazyLoader.Load(this, ref _exams);
            set => _exams = value;
        }
    }
}
