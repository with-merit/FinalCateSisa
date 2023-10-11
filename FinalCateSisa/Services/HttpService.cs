using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FinalCateSisa.Services
{
    public static class HttpService
    {
        private static HttpClient client = new();

        public static async Task<string> MakePredicton(string base64String, string endpointUrl = "http://127.0.0.1:8000/predict")
        {
            try
            {

                var payload = new
                {
                    sample = base64String
                };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpointUrl, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static async Task<string> FGSMAttack(string base64String, int eps = 4, string endpointUrl = "http://127.0.0.1:8000/fgsm")
        {
            try
            {

                var payload = new
                {
                    epsilon = eps,
                    sample = base64String
                };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpointUrl, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
