using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public static class Extensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }

    public static IEnumerable<(BattleChar item, int index)> NotDead(this IEnumerable<(BattleChar item, int index)> source)
    {
        return source.Where(x => x.item.currentHP > 0);
    }

    public static bool AllDead(this IEnumerable<BattleChar> source)
    {
        return source.All(x => x.currentHP <= 0);
    }

    public static IEnumerable<BattleChar> NotDead(this IEnumerable<BattleChar> source)
    {
        return source.Where(x => x.currentHP > 0);
    }

    public static IEnumerable<(BattleChar item, int index)> AllPlayers(this IEnumerable<(BattleChar item, int index)> source)
    {
        return source.Where(x => x.item.IsPlayer);
    }

    public static IEnumerable<BattleChar> AllPlayers(this IEnumerable<BattleChar> source)
    {
        return source.Where(x => x.IsPlayer);
    }

    public static IEnumerable<(BattleChar item, int index)> AllEnemies(this IEnumerable<(BattleChar item, int index)> source)
    {
        return source.Where(x => !x.item.IsPlayer);
    }

    public static IEnumerable<BattleChar> AllEnemies(this IEnumerable<BattleChar> source)
    {
        return source.Where(x => !x.IsPlayer);
    }
}
