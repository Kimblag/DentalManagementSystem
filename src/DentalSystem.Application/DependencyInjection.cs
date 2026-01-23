using DentalSystem.Application.UseCases.Specialties.AddTreatment;
using DentalSystem.Application.UseCases.Specialties.CreateSpecialty;
using DentalSystem.Application.UseCases.Specialties.DeactivateSpecialty;
using DentalSystem.Application.UseCases.Specialties.DeactivateTreatment;
using DentalSystem.Application.UseCases.Specialties.EditSpecialty.CorrectSpecialtyName;
using DentalSystem.Application.UseCases.Specialties.EditSpecialty.UpdateSpecialtyDescription;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.ChangeTreatmentCost;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.CorrectTreatmentName;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.UpdateTreatmentDescription;
using DentalSystem.Application.UseCases.Specialties.ReactivateSpecialty;
using DentalSystem.Application.UseCases.Specialties.ReactivateTreatment;
using Microsoft.Extensions.DependencyInjection;

namespace DentalSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add handlers to the services container
            services.AddScoped<AddTreatmentHandler>();
            services.AddScoped<CreateSpecialtyHandler>();
            services.AddScoped<DeactivateSpecialtyHandler>();
            services.AddScoped<DeactivateTreatmentHandler>();
            services.AddScoped<CorrectSpecialtyNameHandler>();
            services.AddScoped<UpdateSpecialtyDescriptionHandler>();
            services.AddScoped<ChangeTreatmentCostHandler>();
            services.AddScoped<CorrectTreatmentNameHandler>();
            services.AddScoped<UpdateTreatmentDescriptionHandler>();
            services.AddScoped<ReactivateSpecialtyHandler>();
            services.AddScoped<ReactivateTreatmentHandler>();


            return services;
        }
    }
}
