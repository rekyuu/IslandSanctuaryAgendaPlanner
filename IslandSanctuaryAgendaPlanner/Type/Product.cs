using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IslandSanctuaryAgendaPlanner.Type;

[JsonConverter(typeof(StringEnumConverter))]
public enum Product
{
    [EnumMember(Value = "Isleworks Potion")]
    IsleworksPotion,
    
    [EnumMember(Value = "Isleworks Firesand")]
    IsleworksFiresand,
    IsleworksWoodenChair,
    IsleworksGrilledClam,
    IsleworksNecklace,
    IsleworksCoralRing,
    IsleworksBarbut,
    IsleworksMacuahuitl,
    IsleworksSauerkraut,
    IsleworksBakedPumpkin,
    IsleworksTunic,
    IsleworksCulinaryKnife,
    IsleworksBrush,
    IsleworksBoiledEgg,
    IsleworksHora,
    IsleworksEarrings,
    IsleworksButter,
    IsleworksBrickCounter,
    BronzeSheep,
    IsleworksGrowthFormula,
    IsleworksGarnetRapier,
    IsleworksSpruceRoundShield,
    IsleworksSharkOil,
    IsleworksSilverEarCuffs,
    IsleworksSweetPopoto,
    IsleworksParsnipSalad,
    IsleworksCaramels,
    IsleworksRibbon,
    IsleworksRope,
    IsleworksCavaliersHat,
    IsleworksHorn,
    IsleworksSaltCod,
    IsleworksSquidInk,
    IsleworksEssentialDraught,
    IsleberryJam,
    IsleworksTomatoRelish,
    IsleworksOnionSoup,
    IslefishPie,
    IsleworksCornFlakes,
    IsleworksPickledRadish,
    IsleworksIronAxe,
    IsleworksQuartzRing,
    IsleworksPorcelainVase,
    IsleworksVegetableJuice,
    IsleworksPumpkinPudding,
    IsleworksSheepfluffRug,
    IsleworksGardenScythe,
    IsleworksBed,
    IsleworksScaleFingers,
    IsleworksCrook,
}