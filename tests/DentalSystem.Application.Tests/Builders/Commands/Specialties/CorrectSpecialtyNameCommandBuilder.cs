using DentalSystem.Application.UseCases.Specialties.CorrectName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class CorrectSpecialtyNameCommandBuilder
    {
        public static CorrectSpecialtyNameCommand WithName(Guid specialtyId, string name)
        {
            return new CorrectSpecialtyNameCommand
            {
                SpecialtyId = specialtyId,
                Name = name
            };
        }
    }
}
