using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using xiaoye97;

namespace TreePlanterMod {
    public static class Constants {
        public const int StringSaplingID = 28500;
        public const int StringPlantRecipeID = 28501;
        public const int StringPlantRecipeDescID = 28502;
        public const int StringCharcoalID = 28503;
        public const int StringCharcoalDescID = 28504;

        public const int ItemSaplingID = 9150;
        public const int ItemCharcoalID = 9151;

        public const int RecipeSaplingID = 220;
        public const int RecipePlantID = 221;
        public const int RecipeCharcoalID = 222;
    }

    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool")]
    [BepInPlugin("org.bepinex.plugins.treeplantermod", "Tree Planter Mod", "0.0.1")]
    public class TreePlanterMod : BaseUnityPlugin {
        private Sprite _iconCharcoal;
        private Sprite _iconOriWood;
        private Sprite _iconSapling;

        private void Awake() {
            var bundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("TreePlanterMod.resources"));
            _iconSapling = bundle.LoadAsset<Sprite>("iconSapling");
            _iconOriWood = bundle.LoadAsset<Sprite>("oriWood");
            _iconCharcoal = bundle.LoadAsset<Sprite>("charcoal");

            LDBTool.PreAddDataAction += AddTranslate;
            LDBTool.PreAddDataAction += AddSapling;
            LDBTool.PreAddDataAction += AddCharcoal;
            LDBTool.PostAddDataAction += PostLoad;
        }

        private void AddTranslate() {
            var sapling = new StringProto {
                ID = Constants.StringSaplingID,
                Name = "sapling",
                ENUS = "Sapling"
            };
            var plantRecipe = new StringProto {
                ID = Constants.StringPlantRecipeID,
                Name = "plantRecipe",
                ENUS = "Log (original)"
            };
            var plantRecipeDesc = new StringProto {
                ID = Constants.StringPlantRecipeDescID,
                Name = "plantRecipeDesc",
                ENUS = "Growing trees"
            };
            var charcoal = new StringProto {
                ID = Constants.StringCharcoalID,
                Name = "charcoal",
                ENUS = "Charcoal"
            };
            var charcoalDesc = new StringProto {
                ID = Constants.StringCharcoalDescID,
                Name = "charcoalDesc",
                ENUS = "Ordinary fuel. obtained by smelting wood, has the same energy as coal."
            };

            LDBTool.PreAddProto(ProtoType.String, sapling);
            LDBTool.PreAddProto(ProtoType.String, plantRecipe);
            LDBTool.PreAddProto(ProtoType.String, plantRecipeDesc);
            LDBTool.PreAddProto(ProtoType.String, charcoal);
            LDBTool.PreAddProto(ProtoType.String, charcoalDesc);
        }

        private void AddSapling() {
            var saplingRecipe = LDB.recipes.Select(5).Copy();
            saplingRecipe.ID = Constants.RecipeSaplingID;
            saplingRecipe.GridIndex = 1610;
            saplingRecipe.Type = ERecipeType.Assemble;
            saplingRecipe.Name = "sapling";
            saplingRecipe.name = "sapling".Translate();
            saplingRecipe.Items = new[] {1030};
            saplingRecipe.ItemCounts = new[] {1};
            saplingRecipe.Results = new[] {Constants.ItemSaplingID};
            saplingRecipe.ResultCounts = new[] {5};
            saplingRecipe.TimeSpend = 30;
            saplingRecipe.Description = "";
            saplingRecipe.description = "";
            saplingRecipe.preTech = LDB.techs.Select(1121);
            Traverse.Create(saplingRecipe).Field("_iconSprite").SetValue(_iconSapling);

            var plantRecipe = LDB.recipes.Select(23).Copy();
            plantRecipe.ID = Constants.RecipePlantID;
            plantRecipe.GridIndex = 1611;
            plantRecipe.Explicit = true;
            plantRecipe.Type = ERecipeType.Chemical;
            plantRecipe.Name = "plantRecipe";
            plantRecipe.name = plantRecipe.Name.Translate();
            plantRecipe.Items = new[] {Constants.ItemSaplingID, 1000};
            plantRecipe.ItemCounts = new[] {10, 20};
            plantRecipe.Results = new[] {1030, 1031};
            plantRecipe.ResultCounts = new[] {12, 15};
            plantRecipe.TimeSpend = 3600;
            plantRecipe.Description = "plantRecipeDesc";
            plantRecipe.description = plantRecipe.Description.Translate();
            plantRecipe.preTech = LDB.techs.Select(1121);
            Traverse.Create(plantRecipe).Field("_iconSprite").SetValue(_iconOriWood);
            LDB.items.Select(1030).recipes.Add(plantRecipe);
            LDB.items.Select(1031).recipes.Add(plantRecipe);

            var sapling = LDB.items.Select(1201).Copy();
            sapling.ID = Constants.ItemSaplingID;
            sapling.GridIndex = 1606;
            sapling.Type = EItemType.Resource;
            sapling.Name = "sapling";
            sapling.name = sapling.Name.Translate();
            sapling.Description = "";
            sapling.description = "";
            sapling.handcraft = saplingRecipe;
            sapling.handcrafts = new List<RecipeProto> {saplingRecipe};
            sapling.maincraft = saplingRecipe;
            sapling.recipes = new List<RecipeProto> {saplingRecipe};
            sapling.makes = new List<RecipeProto> {plantRecipe};
            Traverse.Create(sapling).Field("_iconSprite").SetValue(_iconSapling);

            LDBTool.PreAddProto(ProtoType.Recipe, saplingRecipe);
            LDBTool.PreAddProto(ProtoType.Recipe, plantRecipe);
            LDBTool.PreAddProto(ProtoType.Item, sapling);
        }

        private void AddCharcoal() {
            var charcoalRecipe = LDB.recipes.Select(17).Copy();
            charcoalRecipe.ID = Constants.RecipeCharcoalID;
            charcoalRecipe.GridIndex = 1612;
            charcoalRecipe.Type = ERecipeType.Smelt;
            charcoalRecipe.Name = "charcoalRecipe";
            charcoalRecipe.name = "";
            charcoalRecipe.Items = new[] {1030};
            charcoalRecipe.ItemCounts = new[] {2};
            charcoalRecipe.Results = new[] {Constants.ItemCharcoalID};
            charcoalRecipe.ResultCounts = new[] {2};
            charcoalRecipe.TimeSpend = 120;
            charcoalRecipe.Description = "";
            charcoalRecipe.description = "";
            charcoalRecipe.preTech = LDB.techs.Select(1401);
            Traverse.Create(charcoalRecipe).Field("_iconSprite").SetValue(_iconCharcoal);

            var charcoal = LDB.items.Select(1109).Copy();
            charcoal.ID = Constants.ItemCharcoalID;
            charcoal.GridIndex = 1607;
            charcoal.Type = EItemType.Resource;
            charcoal.HeatValue = 2700000;
            charcoal.ReactorInc = 0.0f;
            charcoal.Name = "charcoal";
            charcoal.name = charcoal.Name.Translate();
            charcoal.Description = "charcoalDesc";
            charcoal.description = charcoal.Description.Translate();
            charcoal.handcraft = null;
            charcoal.handcrafts = new List<RecipeProto>();
            charcoal.maincraft = charcoalRecipe;
            charcoal.recipes = new List<RecipeProto> {charcoalRecipe};
            charcoal.makes = new List<RecipeProto>();
            Traverse.Create(charcoal).Field("_iconSprite").SetValue(_iconCharcoal);

            LDBTool.PreAddProto(ProtoType.Recipe, charcoalRecipe);
            LDBTool.PreAddProto(ProtoType.Item, charcoal);
        }

        private void PostLoad() {
            ItemProto.InitFuelNeeds();
        }
    }
}