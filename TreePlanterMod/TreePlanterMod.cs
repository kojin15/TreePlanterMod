using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using xiaoye97;

namespace TreePlanterMod {

    //[BepInDependency("Appun.plugins.dspmod.DSPJapanesePlugin", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("me.xiaoye97.plugin.Dyson.LDBTool")]
    [BepInPlugin("org.bepinex.plugins.treeplantermod", "Tree Planter Mod", "0.0.3")]
    public class TreePlanterMod : BaseUnityPlugin {
        //private bool _isLoadedJPPlugin = false;

        private Sprite _iconCharcoal;
        private Sprite _iconOriWood;
        private Sprite _iconSapling;
        
        public StringProto stringSapling;
        public StringProto stringCharcoal;
        public StringProto stringCharcoalDesc;
        public StringProto stringPlantRecipe;
        public StringProto stringPlantRecipeDesc;
        
        public ItemProto itemSapling;
        public ItemProto itemCharcoal;

        public RecipeProto recipeSapling;
        public RecipeProto recipePlant;
        public RecipeProto recipeCharcoal;

        private void Awake() {
            /*
            _isLoadedJPPlugin = BepInEx.Bootstrap.Chainloader.PluginInfos.Values.Any(it =>
                it.Metadata.GUID == "Appun.plugins.dspmod.DSPJapanesePlugin");
            */

            var bundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("TreePlanterMod.resources"));
            _iconSapling = bundle.LoadAsset<Sprite>("iconSapling");
            _iconOriWood = bundle.LoadAsset<Sprite>("oriWood");
            _iconCharcoal = bundle.LoadAsset<Sprite>("charcoal");

            LDBTool.PreAddDataAction += AddTranslate;
            LDBTool.PostAddDataAction += AddItem;
            LDBTool.PostAddDataAction += AddRecipe;
            LDBTool.PostAddDataAction += RegisterRecipeInItem;

            Harmony.CreateAndPatchAll(typeof(TreePlanterMod));
        }
        
        private void AddTranslate() {
            stringSapling = new StringProto {
                ID = 28500,
                Name = "sapling",
                name = "sapling",
                ZHCN = "Sapling",
                ENUS = "Sapling",
                FRFR = "Sapling"
                //JPJP = "苗木"
            };
            stringPlantRecipe = new StringProto {
                ID = 28501,
                Name = "plantRecipe",
                name = "plantRecipe",
                ZHCN = "Log (original)",
                ENUS = "Log (original)",
                FRFR = "Log (original)"
                //JPJP = "木材(原始的)"
            };
            stringPlantRecipeDesc = new StringProto {
                ID = 28502,
                Name = "plantRecipeDesc",
                name = "plantRecipeDesc",
                ZHCN = "Growing trees",
                ENUS = "Growing trees",
                FRFR = "Growing trees"
                //JPJP = "木を育てる"
            };
            stringCharcoal = new StringProto {
                ID = 28503,
                Name = "charcoal",
                name = "charcoal",
                ZHCN = "Charcoal",
                ENUS = "Charcoal",
                FRFR = "Charcoal"
                //JPJP = "木炭"
            };
            stringCharcoalDesc = new StringProto {
                ID = 28504,
                Name = "charcoalDesc",
                name = "charcoalDesc",
                ZHCN = "Ordinary fuel. obtained by smelting wood, has the same energy as coal.",
                ENUS = "Ordinary fuel. obtained by smelting wood, has the same energy as coal.",
                FRFR = "Ordinary fuel. obtained by smelting wood, has the same energy as coal."
                //JPJP = "平凡な燃料です。木を製錬することで得られ、石炭と同等のエネルギーを有します。"
            };

            LDBTool.PreAddProto(ProtoType.String, stringSapling);
            LDBTool.PreAddProto(ProtoType.String, stringPlantRecipe);
            LDBTool.PreAddProto(ProtoType.String, stringPlantRecipeDesc);
            LDBTool.PreAddProto(ProtoType.String, stringCharcoal);
            LDBTool.PreAddProto(ProtoType.String, stringCharcoalDesc);
        }

        private void AddItem() {
            itemSapling = LDB.items.Select(1201).Copy();
            itemSapling.ID = 9150;
            itemSapling.GridIndex = 1506;
            itemSapling.Type = EItemType.Resource;
            itemSapling.Name = "sapling";
            itemSapling.name = itemSapling.Name.Translate();
            itemSapling.Description = "";
            itemSapling.description = "";
            Traverse.Create(itemSapling).Field("_iconSprite").SetValue(_iconSapling);
            LDBTool.PostAddProto(ProtoType.Item, itemSapling);
            
            itemCharcoal = LDB.items.Select(1109).Copy();
            itemCharcoal.ID = 9151;
            itemCharcoal.GridIndex = 1507;
            itemCharcoal.Type = EItemType.Resource;
            itemCharcoal.HeatValue = 2700000;
            itemCharcoal.ReactorInc = 0.0f;
            itemCharcoal.Name = "charcoal";
            itemCharcoal.name = itemCharcoal.Name.Translate();
            itemCharcoal.Description = "charcoalDesc";
            itemCharcoal.description = itemCharcoal.Description.Translate();
            Traverse.Create(itemCharcoal).Field("_iconSprite").SetValue(_iconCharcoal);
            LDBTool.PostAddProto(ProtoType.Item, itemCharcoal);
        }

        private void AddRecipe() {
            recipeSapling = LDB.recipes.Select(5).Copy();
            recipeSapling.ID = 220;
            recipeSapling.GridIndex = 1610;
            recipeSapling.Type = ERecipeType.Assemble;
            recipeSapling.Name = "sapling";
            recipeSapling.name = recipeSapling.Name.Translate();
            recipeSapling.Items = new[] {1030};
            recipeSapling.ItemCounts = new[] {1};
            recipeSapling.Results = new[] {itemSapling.ID};
            recipeSapling.ResultCounts = new[] {5};
            recipeSapling.TimeSpend = 30;
            recipeSapling.Description = "";
            recipeSapling.description = "";
            recipeSapling.preTech = LDB.techs.Select(1121);
            Traverse.Create(recipeSapling).Field("_iconSprite").SetValue(_iconSapling);
            LDBTool.PostAddProto(ProtoType.Recipe, recipeSapling);
            
            recipePlant = LDB.recipes.Select(23).Copy();
            recipePlant.ID = 221;
            recipePlant.GridIndex = 1611;
            recipePlant.Explicit = true;
            recipePlant.Type = ERecipeType.Chemical;
            recipePlant.Name = "plantRecipe";
            recipePlant.name = recipePlant.Name.Translate();
            recipePlant.Items = new[] {itemSapling.ID, 1000};
            recipePlant.ItemCounts = new[] {10, 20};
            recipePlant.Results = new[] {1030, 1031};
            recipePlant.ResultCounts = new[] {12, 15};
            recipePlant.TimeSpend = 3600;
            recipePlant.Description = "plantRecipeDesc";
            recipePlant.description = recipePlant.Description.Translate();
            recipePlant.preTech = LDB.techs.Select(1121);
            Traverse.Create(recipePlant).Field("_iconSprite").SetValue(_iconOriWood);
            LDBTool.PostAddProto(ProtoType.Recipe, recipePlant);
            
            recipeCharcoal = LDB.recipes.Select(17).Copy();
            recipeCharcoal.ID = 222;
            recipeCharcoal.GridIndex = 1612;
            recipeCharcoal.Type = ERecipeType.Smelt;
            recipeCharcoal.Name = "";
            recipeCharcoal.name = "";
            recipeCharcoal.Items = new[] {1030};
            recipeCharcoal.ItemCounts = new[] {2};
            recipeCharcoal.Results = new[] {itemCharcoal.ID};
            recipeCharcoal.ResultCounts = new[] {2};
            recipeCharcoal.TimeSpend = 120;
            recipeCharcoal.Description = "";
            recipeCharcoal.description = "";
            recipeCharcoal.preTech = LDB.techs.Select(1401);
            Traverse.Create(recipeCharcoal).Field("_iconSprite").SetValue(_iconCharcoal);
            LDBTool.PostAddProto(ProtoType.Recipe, recipeCharcoal);
        }

        private void RegisterRecipeInItem() {
            LDB.items.Select(1030).recipes.Add(recipePlant);
            LDB.items.Select(1031).recipes.Add(recipePlant);
            
            itemSapling.handcraft = recipeSapling;
            itemSapling.handcrafts = new List<RecipeProto> {recipeSapling};
            itemSapling.maincraft = recipeSapling;
            itemSapling.recipes = new List<RecipeProto> {recipeSapling};
            itemSapling.makes = new List<RecipeProto> {recipePlant};
            
            itemCharcoal.handcraft = null;
            itemCharcoal.handcrafts = new List<RecipeProto>();
            itemCharcoal.maincraft = recipeCharcoal;
            itemCharcoal.recipes = new List<RecipeProto> {recipeCharcoal};
            itemCharcoal.makes = new List<RecipeProto>();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LDBTool), "VFPreloadPostPatch")]
        private static void PostLoad() {
            ItemProto.InitFuelNeeds();
        }
    }
}