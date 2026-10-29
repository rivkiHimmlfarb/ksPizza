using KsPizza.Models;
using KsPizza.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;


namespace KsPizza.Services
{
    public class PizzaService : IPizzaService
    {
        List<Pizza> Pizzas { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public PizzaService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Pizza.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Pizzas = JsonSerializer.Deserialize<List<Pizza>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Pizzas));
        }
        public List<Pizza> GetAll() => Pizzas;

        public Pizza Get(int id) => Pizzas.FirstOrDefault(p => p.Id == id);

        public void Add(Pizza pizza)
        {
            pizza.Id = Pizzas.Count() + 1;
            Pizzas.Add(pizza);
            saveToFile();
        }

        public void Delete(int id)
        {
            var pizza = Get(id);
            if (pizza is null)
                return;

            Pizzas.Remove(pizza);
            saveToFile();
        }

        public void Update(Pizza pizza)
        {
            var index = Pizzas.FindIndex(p => p.Id == pizza.Id);
            if (index == -1)
                return;

            Pizzas[index] = pizza;
            saveToFile();
        }

        public int Count => Pizzas.Count();
    }
}