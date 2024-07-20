
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Hotel.Models;
using Hotel.Data;

namespace Hotel.Atribute;
[AttributeUsage(AttributeTargets.Property)]
public class UniqueNameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dbContext = (HotelDbContext)validationContext.GetService(typeof(HotelDbContext));

        var existingEntity = dbContext.Set<RoomUnity>()
            .FirstOrDefault(r => r.Name!.ToLower() == (string)value);


        var existingProperty = dbContext.Set<RoomProperty>()
        .FirstOrDefault(r => r.Name!.ToLower() == (string)value);


        var existingType = dbContext.Set<RoomType>()
     .FirstOrDefault(r => r.Type!.ToLower() == (string)value);


        if (existingEntity != null)
        {
            return new ValidationResult("Tên Tiện Ích đã tồn tại.");
        }

        else if (existingProperty != null)
        {
            return new ValidationResult("Tên Property đã tồn tại.");
        }

        else if (existingType != null)
        {
            return new ValidationResult("Tên Type đã tồn tại.");
        }

        return ValidationResult.Success;
    }
}
