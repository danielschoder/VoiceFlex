﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using VoiceFlex.DTO;

namespace VoiceFlex.Models
{
    [Index(nameof(Number), IsUnique = true)]
    public class PhoneNumber : ICallResult
    {
        public Guid Id { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string Number { get; set; }

        public Guid? AccountId { get; set; }
        public virtual Account Account { get; set; }

        public PhoneNumber() { }

        public PhoneNumber(PhoneNumberDto phoneNumber)
            => (Id, Number) = (phoneNumber.Id, phoneNumber.Number);
    }
}
