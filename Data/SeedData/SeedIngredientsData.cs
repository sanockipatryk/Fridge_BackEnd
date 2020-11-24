using Fridge_BackEnd.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fridge_BackEnd.Data.SeedData
{
    public static class SeedIngredientsData
    {
        public static void SeedIngredientsCategoriesData(this ModelBuilder modelBuilder)
        {
            var foodDataFile = File.ReadAllText(@"./Data/SeedData/FoodData.json");
            var foodDataJson = JObject.Parse(foodDataFile);
            var categories = foodDataJson.SelectToken("categories").ToList();
            var categoriesCount = categories.Count();

            var IngredientCategoryList = new List<IngredientCategory>();
            for (int i = 0; i < categoriesCount; i++)
            {
                IngredientCategoryList.Add(new IngredientCategory() { Id = i + 1, Name = categories[i].ToString() });
            }

            modelBuilder.Entity<IngredientCategory>().HasData(
                IngredientCategoryList);
        }

        public static void SeedFoodIngredientsData(this ModelBuilder modelBuilder)
        {
            var foodDataFile = File.ReadAllText(@"./Data/SeedData/FoodData.json");
            var foodDataJson = JObject.Parse(foodDataFile);
            var categories = foodDataJson.SelectToken("categories").ToList();
            var ingredients = foodDataJson.SelectToken("ingredients").ToList();
            var IngredientsList = new List<Ingredient>();
            var ingredientsCount = ingredients.Count();
            ingredients.ForEach(i =>
            {
                var ingredient = new Ingredient()
                {
                    Id = i.SelectToken("id").Value<int>(),
                    Name = i.SelectToken("name").Value<string>(),
                    Description = i.SelectToken("description").Value<string>(),
                    CategoryId = categories.IndexOf(i.SelectToken("food_group").Value<string>()) + 1
                };
                IngredientsList.Add(ingredient);
            });

            modelBuilder.Entity<Ingredient>().HasData(
                IngredientsList);
        }

    }

}
