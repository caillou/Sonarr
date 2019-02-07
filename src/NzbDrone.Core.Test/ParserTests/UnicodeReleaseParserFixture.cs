using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.Indexers;
using NzbDrone.Core.Test.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NzbDrone.Core.Test.ParserTests
{
    [TestFixture]
    public class UnicodeReleaseParserFixture : CoreTest
    {
        [TestCase("【喵萌奶茶屋】★10月新番★[哥布林杀手/Goblin Slayer][12END][720p][繁体][招募翻译校对]", "Goblin Slayer", null, new[] { 12 })]
        [TestCase("[喵萌奶茶屋&LoliHouse]玛娜利亚魔法学院/巴哈姆特之怒Manaria Friends - 03 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕]", "Manaria Friends", "&LoliHouse", new[] { 3 })]
        [TestCase("[桜都字幕组][盾之勇者成名录/Tate no Yuusha no Nariagari][01][BIG5][720P]", "Tate no Yuusha no Nariagari", null, new[] { 1 })]
        [TestCase("[YMDR][輝夜姬想讓人告白～天才們的戀愛頭腦戰～][Kaguya-sama wa Kokurasetai][2019][02][1080p][HEVC][JAP][BIG5][MP4-AAC][繁中]", "Kaguya-sama wa Kokurasetai", "YMDR", new[] {2 })]
        [TestCase("【DHR百合組】[天使降臨到我身邊！_Watashi ni Tenshi ga Maiorita!][05][繁體][1080P10][WebRip][HEVC][MP4]", "Watashi ni Tenshi ga Maiorita!", "DHR", new[] { 5 })]
        [TestCase("[YMDR][慕留人 -火影忍者新時代-][Boruto -Naruto Next Generations-][2017][88-91][1080p][AVC][JAP][BIG5][MP4-AAC][繁中]", "Boruto -Naruto Next Generations", "YMDR", new[] { 88, 89, 90, 91 })]
        [TestCase("[悠哈璃羽字幕社&拉斯观测组&LoliHouse] 刀剑神域: Alicization / Sword Art Online: Alicization - 17 [WebRip 1080p HEVC-10bit AAC][简繁内封字幕]", "Sword Art Online: Alicization", "&&LoliHouse", new[] { 17 })]
        public void should_parse_chinese_anime_releases(string postTitle, string title, string subgroup, int[] absoluteEpisodeNumbers)
        {
            // Rss Parser cleans up the title partially.
            postTitle = XmlCleaner.ReplaceUnicode(postTitle);

            var result = Parser.Parser.ParseTitle(postTitle);
            result.Should().NotBeNull();
            result.ReleaseGroup.Should().Be(subgroup);
            result.AbsoluteEpisodeNumbers.Should().BeEquivalentTo(absoluteEpisodeNumbers);
            result.SeriesTitle.Should().Be(title);
            result.FullSeason.Should().BeFalse();
        }
    }
}
