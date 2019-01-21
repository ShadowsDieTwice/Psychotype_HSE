using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Psychotype_HSE.Models.Components
{
	public class RussianStemmer
	{
		private static Regex PERFECTIVEGROUND = new Regex("((ив|ивши|ившись|ыв|ывши|ывшись)|((<;=[ая])(в|вши|вшись)))$");
		private static Regex REFLEXIVE = new Regex("(с[яь])$");
		private static Regex ADJECTIVE = new Regex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$");
		private static Regex PARTICIPLE = new Regex("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$");
		private static Regex VERB = new Regex("((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$");
		private static Regex NOUN = new Regex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");
		private static Regex RVRE = new Regex("^(.*?[аеиоуыэюя])(.*)$");
		private static Regex DERIVATIONAL = new Regex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$");
		private static Regex DER = new Regex("ость?$");
		private static Regex SUPERLATIVE = new Regex("(ейше|ейш)$");
		private static Regex I = new Regex("и$");
		private static Regex P = new Regex("ь$");
		private static Regex NN = new Regex("нн$");

		public static string GetTheBase(string word)
		{
			word = word.ToLower();
			word = word.Replace('ё', 'е');
			MatchCollection m = RVRE.Matches(word);
			if (m.Count > 0)
			{
				Match match = m[0];
				GroupCollection groupCollection = match.Groups;
				string pre = groupCollection[1].ToString();
				string rv = groupCollection[2].ToString();
				MatchCollection temp = PERFECTIVEGROUND.Matches(rv);
				string StringTemp = Replace1(temp, rv);
				if (StringTemp.Equals(rv))
				{
					MatchCollection tempRV = REFLEXIVE.Matches(rv);
					rv = Replace1(tempRV, rv);
					temp = ADJECTIVE.Matches(rv);
					StringTemp = Replace1(temp, rv);
					if (!StringTemp.Equals(rv))
					{
						rv = StringTemp;
						tempRV = PARTICIPLE.Matches(rv);
						rv = Replace1(tempRV, rv);
					}
					else
					{
						temp = VERB.Matches(rv);
						StringTemp = Replace1(temp, rv);
						if (StringTemp.Equals(rv))
						{
							tempRV = NOUN.Matches(rv);
							rv = Replace1(tempRV, rv);
						}
						else
						{
							rv = StringTemp;
						}
					}
				}
				else
				{
					rv = StringTemp;
				}
				MatchCollection tempRv = I.Matches(rv);
				rv = Replace1(tempRv, rv);
				if (DERIVATIONAL.Matches(rv).Count > 0)
				{
					tempRv = DER.Matches(rv);
					rv = Replace1(tempRv, rv);
				}
				temp = P.Matches(rv);
				StringTemp = Replace1(temp, rv);
				if (StringTemp.Equals(rv))
				{
					tempRv = SUPERLATIVE.Matches(rv);
					rv = Replace1(tempRv, rv);
					tempRv = NN.Matches(rv);
					rv = Replace1(tempRv, rv);
				}
				else
				{
					rv = StringTemp;
				}
				word = pre + rv;
			}
			return word;
		}
		public static string Replace1(MatchCollection collection, string part)
		{
			string StringTemp = "";
			if (collection.Count == 0)
			{
				return part;
			}

			StringTemp = part;
			for (int i = 0; i < collection.Count; i++)
			{
				GroupCollection GroupCollection = collection[i].Groups;
				if (StringTemp.Contains(GroupCollection[i].ToString()))
				{
					string deletePart = GroupCollection[i].ToString();
					StringTemp = StringTemp.Replace(deletePart, "");
				}
			}

			return StringTemp;
		}
	}
}