using NUnit.Framework;

using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitterAPI;

namespace TwitterAPITests
{
    [TestFixture]
    public class Tests
    {


        [SetUp]
        public void Setup()
        {
            

        }


        [Test]
        public void TweetsWithEmoji()
        {            
            Assert.AreEqual(true, TwitterAPI.Tweets.eMojiStat("@LikeChrisss @JoutheJoseph 😂😂😂😂"));           
        }

        [Test]
        public void TweetsWithOutEmoji()
        {
            Assert.AreEqual(false, TwitterAPI.Tweets.eMojiStat("@LikeChrisss @JLSJLK jalkdlaikjdlkajdlkasdjlaksdjlaksj"));
        }

        [Test]
        public void TweetsWithHashtag()
        {
            Assert.AreEqual(true, TwitterAPI.Tweets.HashTagStat("askdjhaskdjhas #luckyguy jklsjdlakjasd"));
        }

        [Test]
        public void TweetsWithOutHashtag()
        {
            Assert.AreEqual(false, TwitterAPI.Tweets.HashTagStat("askdjhaskdjhas#!luckyguy jklsjdlakjasd"));
        }

        [Test]
        public void TweetsWithURL()
        {
            Assert.AreEqual(true, TwitterAPI.Tweets.URLStat("askdjhaswww.hotmail.comkdjhas#luckyguy jklsjdlakjasd"));
        }

        [Test]
        public void TweetsWithOutURL()
        {
            Assert.AreEqual(false, TwitterAPI.Tweets.URLStat("askdjhaswtwwy..hotmail..cmommkdjhas jklsjdlakjasd"));
        }


        [Test]
        public void Getdomain()
        {
            Assert.AreEqual("microsoft.com", TwitterAPI.Tweets.getDomainName("https://www.microsoft.com/en-us/store/b/xbox"));
        }


        [Test]
        public void TweetsWithPics()
        {
            Assert.AreEqual(true, TwitterAPI.Tweets.PICStat("askdjhapic.twitter.comkyguy jklsjdlakjasd"));
        }

        [Test]
        public void TweetsWithOutPics()
        {
            Assert.AreEqual(false, TwitterAPI.Tweets.PICStat("askdjhaswtwwy..hotmail..cmommkdjhas jklsjdlakjasd"));
        }


    }
}