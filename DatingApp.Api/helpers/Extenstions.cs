using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Api.helpers
{
    public static class Extenstions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // add a new header called application Error
            response.Headers.Add("Application-Error", message);
            // Allow these to be displayed
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");

        }
        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if(theDateTime.AddYears(age) > DateTime.Today)
                age--;
            
            return age;
        }
    }
}