using HarmonyLib;
using Redbox.Rental.Model.Browse;
using Redbox.Rental.Model.KioskProduct;
using Redbox.Rental.Services.Controllers;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;
using System;

namespace RedboxPatches.RedboxRentalServices
{
    [HarmonyPatch(typeof(BrowseViewController))]
    [HarmonyPatch("CreateMenuButtons")]
    public class GamesButtonPatch
    {
        private static readonly MethodInfo createProductFamilyMenuButtonMethod = AccessTools.Method(typeof(BrowseViewController), "CreateProductFamilyMenuButton", new[] { typeof(TitleFamily), typeof(ArrowDirection) });

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);
            int addRangeIndex = -1;
            int top20index = -1;
            int top20VariableIndex = -1;

            for (int i = 0; i < code.Count; i++)
            {
                // Debugging each instruction to confirm position
                //Console.WriteLine($"Instruction {i}: {code[i]}");

                // Detect Top20Movies button creation sequence
                if (code[i].opcode == OpCodes.Ldc_I4 && (int)code[i].operand == 1093 && // Genres.Top20Movies
                    code[i + 1].opcode == OpCodes.Conv_I8 &&
                    code[i + 2].opcode == OpCodes.Newobj &&
                    code[i + 3].opcode == OpCodes.Ldloc_0 && // highlightSelectedGenre
                    code[i + 4].opcode == OpCodes.Ldloc_S && // width
                    code[i + 5].opcode == OpCodes.Ldloc_S && // isDeselectable
                    code[i + 6].opcode == OpCodes.Call && ((MethodInfo)code[i + 6].operand).Name == "CreateGenreMenuButton" &&
                    code[i + 7].opcode == OpCodes.Stloc_S)
                {
                    top20index = i;
                    Console.WriteLine("Found Top20Movies button creation at index " + i);
                }

                if (top20index != -1 && i > top20index && code[i].opcode == OpCodes.Ldloc_S && code[i + 1].opcode == OpCodes.Callvirt && code[i + 2].opcode == OpCodes.Dup)
                {
                    top20VariableIndex = i;
                    Console.WriteLine("Found Top20Movies button variable at index " + i);
                }

                // Detect AddRange call for MenuButtons list
                if (code[i].opcode == OpCodes.Ldc_I4 && (int)code[i].operand == 1000)
                {
                    addRangeIndex = i;
                    Console.WriteLine("Found AddRange at index " + i);
                }
            }

            if (top20index != -1)
            {
                Console.WriteLine("Removing Top20 button");

                // Remove Top20Movies button creation instructions
                code.RemoveRange(top20index, 8);

                // adjust other indices
                if (top20VariableIndex != -1) top20VariableIndex -= 8;
                if (addRangeIndex != -1) addRangeIndex -= 8;
            }
            else
            {
                Console.WriteLine("Failed to find Top20 index");
            }

            if (addRangeIndex != -1)
            {
                Console.WriteLine("Adding Games button");

                code.Insert(addRangeIndex, new CodeInstruction(OpCodes.Ldc_I4, (int)TitleFamily.Games)); // Load TitleFamily.Games
                code.Insert(addRangeIndex + 1, new CodeInstruction(OpCodes.Ldc_I4, (int)ArrowDirection.Left)); // Load ArrowDirection.Left
                code.Insert(addRangeIndex + 2, new CodeInstruction(OpCodes.Call, createProductFamilyMenuButtonMethod)); // Call CreateProductFamilyMenuButton
                code.Insert(addRangeIndex + 3, new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<BrowseMenuButton>), "Add"))); // Add to MenuButtons list
                code.Insert(addRangeIndex + 4, new CodeInstruction(OpCodes.Dup));

                // adjust other indices
                if (top20VariableIndex != -1) top20VariableIndex += 5;
            }
            else
            {
                Console.WriteLine("Failed to find AddRange index");
            }

            if (top20VariableIndex != -1)
            {
                Console.WriteLine("Removing Top20 button variable");

                code.RemoveRange(top20VariableIndex, 3);
            }
            else
            {
                Console.WriteLine("Failed to find Top20 button variable index");
            }

            return code.AsEnumerable();
        }
    }
}
