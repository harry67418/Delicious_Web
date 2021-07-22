using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Models
{
    //public class CSearch
    //{
    //    public int[] RecipeSearches(DeliciousContext DBContext, string SearchString)
    //    {
    //        string[] Searches = SearchString.Split();
    //        var Recipes = DBContext.Trecipes.Select(r => new
    //        {
    //            r.RecipeId,
    //            r.RecipeName,
    //            RecipeCategory = DBContext.TrecipeCategories.FirstOrDefault(c => c.RecipeCategoryId == r.RecipeCategoryId).Select(a => a.RecipeCategory),
    //        };
    //        foreach(string s in Searches)
    //        {

    //        }
    //        return null;
    //    }
    //}
    class SearchByTagClass
    {
        //public int[] TagsStringToTagIds(DeliciousContext DBContext, string TagsString)
        //{
        //    var dbc = DBContext;
        //    string[] tags = TagsString.Split();
        //    int[] tagids = dbc.Hashtag_Table.Where(t => tags.Contains(t.Hasgtag)).Select(t => t.HashtagID).ToArray<int>();
        //    return tagids;
        //}
        //public int[] TagsStringToTagIds(DeliciousEntities DBContext, string TagsString, bool fuzzy = (false))
        //{
        //    if (fuzzy)
        //    {
        //        var dbc = DBContext;
        //        char[] tags = TagsString.ToCharArray();
        //        string[] stc = tags.Select(c => c.ToString()).Distinct().ToArray();
        //        var ttsl = dbc.Hashtag_Table.ToList();
        //        var tts = ttsl.Select(t => new
        //        {
        //            t.HashtagID,
        //            ttcs = t.Hasgtag.ToCharArray().Select(c => c.ToString()).Distinct().ToArray(),
        //        }).ToList();
        //        int[] tagids = tts.Where(tt => tt.ttcs.Where(ttc => stc.Contains(ttc)).Count() != 0).Select(t => t.HashtagID).ToArray();
        //        return tagids;
        //    }
        //    else
        //    {
        //        return TagsStringToTagIds(DBContext, TagsString);
        //    }

        //}
        //public int[] TagIdsToRecipeIds(DeliciousEntities DBContext, int[] TagIds)
        //{
        //    var dbc = DBContext;
        //    int[] recipeids = dbc.Hashtag_Record_Table.GroupBy(r => r.RecipeID).Where(g => TagIds.All(t => g.Where(r => r.HashTagID == t).Count() != 0)).Select(g => g.Key).ToArray<int>();
        //    return recipeids;
        //}
        //public int[] TagIdsToRecipeIds(DeliciousEntities DBContext, int[] TagIds, bool fuzzy = (false))
        //{
        //    if (fuzzy)
        //    {
        //        var dbc = DBContext;
        //        int[] recipeids = dbc.Hashtag_Record_Table.GroupBy(r => r.RecipeID).Where(g => TagIds.Any(t => g.Where(r => r.HashTagID == t).Count() != 0)).Select(g => g.Key).ToArray<int>();
        //        return recipeids;
        //    }
        //    else
        //    {
        //        return TagIdsToRecipeIds(DBContext, TagIds);
        //    }
        //}
        //public int[] FindAdviceTags(DeliciousEntities DBContext, string TagsString)
        //{
        //    var dbc = DBContext;
        //    string LastTag = TagsString.Split().LastOrDefault();
        //    char[] tags = LastTag.ToCharArray();
        //    string[] stc = tags.Select(c => c.ToString()).Distinct().ToArray();
        //    var ttsl = dbc.Hashtag_Table.ToList();
        //    var tts = ttsl.Select(t => new
        //    {
        //        t.HashtagID,
        //        ttcs = t.Hasgtag.ToCharArray().Select(c => c.ToString()).Distinct().ToArray(),
        //    }).ToList();
        //    int[] tagids = tts.Where(tt => tt.ttcs.Where(ttc => stc.Contains(ttc)).Count() != 0).Select(t => t.HashtagID).ToArray();
        //    if (dbc.Hashtag_Table.Where(t => t.Hasgtag == LastTag).ToList().Count() != 0) tagids = new int[] { };
        //    return tagids;
        //}
        //public void TagTips(DeliciousEntities DBContext, TextBox textBox, ToolTip toolTip)
        //{
        //    DeliciousEntities dbc = DBContext;
        //    int[] tagids = this.FindAdviceTags(dbc, textBox.Text);
        //    string LastTag = textBox.Text.Split().LastOrDefault();
        //    if (dbc.Hashtag_Table.Where(t => t.Hasgtag == LastTag).ToList().Count() == 0)
        //    {
        //        if (tagids.Count() != 0)
        //        {
        //            var atags = tagids.Select(i => dbc.Hashtag_Table.Where(t => t.HashtagID == i).Select(t => t.Hasgtag).FirstOrDefault()).ToList();
        //            string tips = "您是不是要輸入:";
        //            foreach (string atag in atags)
        //            {
        //                tips += "#" + atag + " ";
        //            }
        //            toolTip.SetToolTip(textBox, tips);
        //        }
        //        else
        //        {
        //            toolTip.SetToolTip(textBox, "請輸入HashTag");
        //        }
        //    }
        //    else toolTip.SetToolTip(textBox, "您輸入了:#" + LastTag);
        //}
        //public int[] IngdStringToIngdIds(DeliciousEntities DBContext, string IngdsString)
        //{
        //    var dbc = DBContext;
        //    string[] tags = IngdsString.Split();
        //    int[] tagids = dbc.Ingredient_Table.Where(t => tags.Contains(t.Ingredient)).Select(t => t.IngredientID).ToArray<int>();
        //    return tagids;
        //}
        //public int[] IngdStringToIngdIds(DeliciousEntities DBContext, string IngdsString, bool fuzzy = (false))
        //{
        //    if (fuzzy)
        //    {
        //        var dbc = DBContext;
        //        char[] tags = IngdsString.ToCharArray();
        //        string[] stc = tags.Select(c => c.ToString()).Distinct().ToArray();
        //        var ttsl = dbc.Ingredient_Table.ToList();
        //        var tts = ttsl.Select(t => new
        //        {
        //            t.IngredientID,
        //            ttcs = t.Ingredient.ToCharArray().Select(c => c.ToString()).Distinct().ToArray(),
        //        }).ToList();
        //        int[] tagids = tts.Where(tt => tt.ttcs.Where(ttc => stc.Contains(ttc)).Count() != 0).Select(t => t.IngredientID).ToArray();
        //        return tagids;
        //    }
        //    else
        //    {
        //        return IngdStringToIngdIds(DBContext, IngdsString);
        //    }

        //}
        //public int[] IngdIdsToRecipeIds(DeliciousEntities DBContext, int[] TagIds)
        //{
        //    var dbc = DBContext;
        //    int[] recipeids = dbc.Ingredient_Record_Table.GroupBy(r => r.RecipeID).Where(g => TagIds.All(t => g.Where(r => r.IngredientID == t).Count() != 0)).Select(g => g.Key).ToArray<int>();
        //    return recipeids;
        //}
        //public int[] IngdIdsToRecipeIds(DeliciousEntities DBContext, int[] TagIds, bool fuzzy = (false))
        //{
        //    if (fuzzy)
        //    {
        //        var dbc = DBContext;
        //        int[] recipeids = dbc.Ingredient_Record_Table.GroupBy(r => r.RecipeID).Where(g => TagIds.Any(t => g.Where(r => r.IngredientID == t).Count() != 0)).Select(g => g.Key).ToArray<int>();
        //        return recipeids;
        //    }
        //    else
        //    {
        //        return IngdIdsToRecipeIds(DBContext, TagIds);
        //    }
        //}
        //public int[] FindAdviceIngds(DeliciousEntities DBContext, string TagsString)
        //{
        //    var dbc = DBContext;
        //    string LastTag = TagsString.Split().LastOrDefault();
        //    char[] tags = LastTag.ToCharArray();
        //    string[] stc = tags.Select(c => c.ToString()).Distinct().ToArray();
        //    var ttsl = dbc.Ingredient_Table.ToList();
        //    var tts = ttsl.Select(t => new
        //    {
        //        t.IngredientID,
        //        ttcs = t.Ingredient.ToCharArray().Select(c => c.ToString()).Distinct().ToArray(),
        //    }).ToList();
        //    int[] tagids = tts.Where(tt => tt.ttcs.Where(ttc => stc.Contains(ttc)).Count() != 0).Select(t => t.IngredientID).ToArray();
        //    if (dbc.Ingredient_Table.Where(t => t.Ingredient == LastTag).ToList().Count() != 0) tagids = new int[] { };
        //    return tagids;
        //}
        //public void IngdTips(DeliciousEntities DBContext, TextBox textBox, ToolTip toolTip)
        //{
        //    DeliciousEntities dbc = DBContext;
        //    int[] tagids = this.FindAdviceIngds(dbc, textBox.Text);
        //    string LastTag = textBox.Text.Split().LastOrDefault();
        //    if (dbc.Ingredient_Table.Where(t => t.Ingredient == LastTag).ToList().Count() == 0)
        //    {
        //        if (tagids.Count() != 0)
        //        {
        //            var atags = tagids.Select(i => dbc.Ingredient_Table.Where(t => t.IngredientID == i).Select(t => t.Ingredient).FirstOrDefault()).ToList().Take(8);
        //            string tips = "您是不是要輸入:";
        //            foreach (string atag in atags)
        //            {
        //                tips += atag + " ";
        //            }
        //            toolTip.SetToolTip(textBox, tips);
        //        }
        //        else
        //        {
        //            toolTip.SetToolTip(textBox, "請輸入食材");
        //        }
        //    }
        //    else toolTip.SetToolTip(textBox, "您輸入了:" + LastTag);
        //}
        //public int[] RcpStringToRcpIds(DeliciousEntities DBContext, string TagsString)
        //{
        //    var dbc = DBContext;
        //    string[] tags = TagsString.Split();
        //    int[] tagids = dbc.Recipe_Table.Where(t => tags.Contains(t.RecipeName)).Select(t => t.RecipeID).ToArray<int>();
        //    return tagids;
        //}
        //public int[] RcpStringToRcpIds(DeliciousEntities DBContext, string TagsString, bool fuzzy = (false))
        //{
        //    if (fuzzy)
        //    {
        //        var dbc = DBContext;
        //        char[] tags = TagsString.ToCharArray();
        //        string[] stc = tags.Select(c => c.ToString()).Distinct().ToArray();
        //        var ttsl = dbc.Recipe_Table.ToList();
        //        var tts = ttsl.Select(t => new
        //        {
        //            t.RecipeID,
        //            ttcs = t.RecipeName.ToCharArray().Select(c => c.ToString()).Distinct().ToArray(),
        //        }).ToList();
        //        int[] tagids = tts.Where(tt => tt.ttcs.Where(ttc => stc.Contains(ttc)).Count() != 0).Select(t => t.RecipeID).ToArray();
        //        return tagids;
        //    }
        //    else
        //    {
        //        return RcpStringToRcpIds(DBContext, TagsString);
        //    }
        //}
    }
}
