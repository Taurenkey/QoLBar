using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace QoLBar.Conditions;

public class BuffCondition : ICondition, IDrawableCondition, IConditionCategory
{
    public const string constID = "buff";
    private static readonly List<Lumina.Excel.GeneratedSheets.Status> buffs = DalamudApi.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Status>().Where(x => x.StatusCategory == 1 && x.Name.RawString.Length > 0).ToList();

    public string ID => constID;
    public string ConditionName => "Buff";
    public string CategoryName => "Buff";
    public int DisplayPriority => 0;
    public bool Check(dynamic arg) => DalamudApi.ClientState.LocalPlayer.StatusList.Any(x => x.StatusId == arg);
    public string GetTooltip(CndCfg cndCfg) => null;
    public string GetSelectableTooltip(CndCfg cndCfg) => null;
    public void Draw(CndCfg cndCfg)
    {
        string prev = buffs.Any(x => x.RowId == cndCfg.Arg) ? $"[{buffs.First(x => x.RowId == cndCfg.Arg).RowId}] {buffs.First(x => x.RowId == cndCfg.Arg).Name.RawString}" : "";
        if (!ImGui.BeginCombo("##Buff", prev)) return;
        if (ImGui.Selectable($"", cndCfg.Arg == 0))
        {
            cndCfg.Arg = 0;
            QoLBar.Config.Save();
        }

        foreach (var status in buffs.OrderBy(x => x.Name.RawString))
        {
            if (!ImGui.Selectable($"[{status.RowId}] {status.Name.RawString}", (int)status.RowId == cndCfg.Arg)) continue;

            cndCfg.Arg = status.RowId;
            QoLBar.Config.Save();
        }
        ImGui.EndCombo();
    }
}