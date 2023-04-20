﻿using Avalonia.PropertyGrid.Model.Collections;
using Avalonia.PropertyGrid.Model.ComponentModel.DataAnnotations;
using Avalonia.PropertyGrid.Model.Extensions;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Avalonia.PropertyGrid.Samples.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        readonly SimpleObject _SimpleObject = new SimpleObject();

        public SimpleObject simpleObject => _SimpleObject;
    }

    public class SimpleObject
    {
        [Category("String")]
        [DisplayName("Target Name")]
        public string Name { get; set; }

        [Category("String")]
        [DisplayName("Target Path")]
        [PathBrowsable(Filters = "Image Files(*.jpg;*.png;*.bmp;*.tag)|*.jpg;*.png;*.bmp;*.tag")]
        public string Path { get; set; }

        [Category("String")]
        public string UUID { get; set; } = Guid.NewGuid().ToString();

        [Category("String")]
        [PasswordPropertyText(true)]
        public string Password { get; set; }

        [Category("Boolean")]
        public bool EncryptData { get; set; } = true;

        [Category("Boolean")]
        public bool SafeMode { get; set; } = false;

        [Category("Boolean")]
        public bool? ThreeStates { get; set; } = null;

        [Category("Enum")]
        public PhoneService Service { get; set; } = PhoneService.None;

        [Category("Enum")]
        public PlatformID CurrentPlatform => Environment.OSVersion.Platform;

        [Category("Enum")]
        public PlatformID Platform { get; set; } = Environment.OSVersion.Platform;

        [Category("Selectable List")]
        public SelectableList<string> LoginName { get; set; } = new SelectableList<string>(new string[] { "John", "David", "bodong" }, "bodong");

        string _SourceImagePath;
        [Category("DataValidation")]
        [PathBrowsable(Filters = "Image Files(*.jpg;*.png;*.bmp;*.tag)|*.jpg;*.png;*.bmp;*.tag")]
        public string SourceImagePath
        {
            get => _SourceImagePath;
            set
            {
                if(value.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(nameof(SourceImagePath));
                }

                if(!System.IO.Path.GetExtension(value).iEquals(".png"))
                {
                    throw new ArgumentException($"{nameof(SourceImagePath)} must be .png file.");
                }

                _SourceImagePath = value;
            }
        }

        [Category("DataValidation")]
        [Description("Select platforms")]
        [ValidatePlatform]
        public CheckedList<PlatformID> Platforms { get; set; } = new CheckedList<PlatformID>(Enum.GetValues(typeof(PlatformID)).Cast<PlatformID>());

        [Category("Numeric")]
        [Range(10, 200)]
        public int iValue { get; set; } = 100;

        [Category("Numeric")]
        [Range(0.1f, 10.0f)]
        public float fValue { get; set; } = 0.5f;

        [Category("Numeric")]
        public Int64 i64Value { get; set; } = 1000000000;

        [Category("Binding List")]
        public BindingList<string> stringList { get; set; } = new BindingList<string>() { "bodong", "china" };

        [Category("Binding List")]
        public BindingList<Boolean> boolList { get; set; } = new BindingList<bool> { true, false };
    }

    [Flags]
    public enum PhoneService
    {
        None = 0,
        LandLine = 1,
        Cell = 2,
        Fax = 4,
        Internet = 8,
        Other = 16
    }

    public class ValidatePlatformAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is CheckedList<PlatformID> id)
            {
                if(id.Contains(PlatformID.Unix) || id.Contains(PlatformID.Other))
                {
                    return new ValidationResult("Can't select Unix or Other");
                }
            }

            return ValidationResult.Success;
        }
    }

}