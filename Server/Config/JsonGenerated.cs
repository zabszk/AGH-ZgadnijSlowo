#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace Utf8Json.Resolvers
{
    using System;
    using Utf8Json;

    public class GeneratedResolver : global::Utf8Json.IJsonFormatterResolver
    {
        public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::Utf8Json.IJsonFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(19)
            {
                {typeof(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>), 0 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>), 1 },
                {typeof(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardUser>), 2 },
                {typeof(global::System.Collections.Generic.List<string>), 3 },
                {typeof(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardGame>), 4 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.User>), 5 },
                {typeof(global::System.Collections.Generic.Dictionary<string, int>), 6 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.OldJsonObjects.User>), 7 },
                {typeof(global::Server.Config.JsonObjects.Round), 8 },
                {typeof(global::Server.Config.JsonObjects.CurrentConfig), 9 },
                {typeof(global::Server.Config.JsonObjects.UserRound), 10 },
                {typeof(global::Server.Config.JsonObjects.ScoreboardUser), 11 },
                {typeof(global::Server.Config.JsonObjects.ScoreboardGame), 12 },
                {typeof(global::Server.Config.JsonObjects.Scoreboard), 13 },
                {typeof(global::Server.Config.JsonObjects.PrimaryConfig), 14 },
                {typeof(global::Server.Config.JsonObjects.User), 15 },
                {typeof(global::Server.Config.JsonObjects.UsersConfig), 16 },
                {typeof(global::Server.Config.JsonObjects.OldJsonObjects.User), 17 },
                {typeof(global::Server.Config.JsonObjects.OldJsonObjects.OldUsersConfig), 18 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ListFormatter<global::Server.Config.JsonObjects.Round>();
                case 1: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Server.Config.JsonObjects.UserRound>();
                case 2: return new global::Utf8Json.Formatters.ListFormatter<global::Server.Config.JsonObjects.ScoreboardUser>();
                case 3: return new global::Utf8Json.Formatters.ListFormatter<string>();
                case 4: return new global::Utf8Json.Formatters.ListFormatter<global::Server.Config.JsonObjects.ScoreboardGame>();
                case 5: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Server.Config.JsonObjects.User>();
                case 6: return new global::Utf8Json.Formatters.DictionaryFormatter<string, int>();
                case 7: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Server.Config.JsonObjects.OldJsonObjects.User>();
                case 8: return new Utf8Json.Formatters.Server.Config.JsonObjects.RoundFormatter();
                case 9: return new Utf8Json.Formatters.Server.Config.JsonObjects.CurrentConfigFormatter();
                case 10: return new Utf8Json.Formatters.Server.Config.JsonObjects.UserRoundFormatter();
                case 11: return new Utf8Json.Formatters.Server.Config.JsonObjects.ScoreboardUserFormatter();
                case 12: return new Utf8Json.Formatters.Server.Config.JsonObjects.ScoreboardGameFormatter();
                case 13: return new Utf8Json.Formatters.Server.Config.JsonObjects.ScoreboardFormatter();
                case 14: return new Utf8Json.Formatters.Server.Config.JsonObjects.PrimaryConfigFormatter();
                case 15: return new Utf8Json.Formatters.Server.Config.JsonObjects.UserFormatter();
                case 16: return new Utf8Json.Formatters.Server.Config.JsonObjects.UsersConfigFormatter();
                case 17: return new Utf8Json.Formatters.Server.Config.JsonObjects.OldJsonObjects.UserFormatter();
                case 18: return new Utf8Json.Formatters.Server.Config.JsonObjects.OldJsonObjects.OldUsersConfigFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning disable 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Utf8Json.Formatters.Server.Config.JsonObjects
{
    using System;
    using Utf8Json;


    public sealed class RoundFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.Round>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public RoundFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ShortName"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Name"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("DisplayOrder"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ShortName"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Name"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("DisplayOrder"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.Round value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ShortName);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.Name);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteInt32(value.DisplayOrder);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.Round Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __ShortName__ = default(string);
            var __ShortName__b__ = false;
            var __Name__ = default(string);
            var __Name__b__ = false;
            var __DisplayOrder__ = default(int);
            var __DisplayOrder__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __ShortName__ = reader.ReadString();
                        __ShortName__b__ = true;
                        break;
                    case 1:
                        __Name__ = reader.ReadString();
                        __Name__b__ = true;
                        break;
                    case 2:
                        __DisplayOrder__ = reader.ReadInt32();
                        __DisplayOrder__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.Round(__ShortName__, __Name__, __DisplayOrder__);

            return ____result;
        }
    }


    public sealed class CurrentConfigFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.CurrentConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public CurrentConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ActiveRound"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlayersLimit"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("MinimumPlayersAmount"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameDelay"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ActiveRound"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlayersLimit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("MinimumPlayersAmount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameDelay"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.CurrentConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ActiveRound);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt16(value.PlayersLimit);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteUInt16(value.MinimumPlayersAmount);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteUInt16(value.GameDelay);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.CurrentConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __ActiveRound__ = default(string);
            var __ActiveRound__b__ = false;
            var __PlayersLimit__ = default(ushort);
            var __PlayersLimit__b__ = false;
            var __MinimumPlayersAmount__ = default(ushort);
            var __MinimumPlayersAmount__b__ = false;
            var __GameDelay__ = default(ushort);
            var __GameDelay__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __ActiveRound__ = reader.ReadString();
                        __ActiveRound__b__ = true;
                        break;
                    case 1:
                        __PlayersLimit__ = reader.ReadUInt16();
                        __PlayersLimit__b__ = true;
                        break;
                    case 2:
                        __MinimumPlayersAmount__ = reader.ReadUInt16();
                        __MinimumPlayersAmount__b__ = true;
                        break;
                    case 3:
                        __GameDelay__ = reader.ReadUInt16();
                        __GameDelay__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.CurrentConfig(__ActiveRound__, __PlayersLimit__, __MinimumPlayersAmount__, __GameDelay__);

            return ____result;
        }
    }


    public sealed class UserRoundFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.UserRound>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserRoundFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Score"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Games"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Score"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Games"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.UserRound value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteUInt32(value.Score);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt32(value.Games);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.UserRound Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __Score__ = default(uint);
            var __Score__b__ = false;
            var __Games__ = default(uint);
            var __Games__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Score__ = reader.ReadUInt32();
                        __Score__b__ = true;
                        break;
                    case 1:
                        __Games__ = reader.ReadUInt32();
                        __Games__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.UserRound(__Score__, __Games__);

            return ____result;
        }
    }


    public sealed class ScoreboardUserFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.ScoreboardUser>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ScoreboardUserFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Username"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Score"), 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Username"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Score"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.ScoreboardUser value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Username);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>>().Serialize(ref writer, value.Score, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.ScoreboardUser Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __Username__ = default(string);
            var __Username__b__ = false;
            var __Score__ = default(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>);
            var __Score__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Username__ = reader.ReadString();
                        __Username__b__ = true;
                        break;
                    case 1:
                        __Score__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>>().Deserialize(ref reader, formatterResolver);
                        __Score__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.ScoreboardUser(__Username__, __Score__);

            return ____result;
        }
    }


    public sealed class ScoreboardGameFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.ScoreboardGame>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ScoreboardGameFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("InternalId"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("TimeElapsed"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Players"), 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("InternalId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("TimeElapsed"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Players"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.ScoreboardGame value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteUInt32(value.InternalId);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteInt32(value.TimeElapsed);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Serialize(ref writer, value.Players, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.ScoreboardGame Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __InternalId__ = default(uint);
            var __InternalId__b__ = false;
            var __TimeElapsed__ = default(int);
            var __TimeElapsed__b__ = false;
            var __Players__ = default(global::System.Collections.Generic.List<string>);
            var __Players__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __InternalId__ = reader.ReadUInt32();
                        __InternalId__b__ = true;
                        break;
                    case 1:
                        __TimeElapsed__ = reader.ReadInt32();
                        __TimeElapsed__b__ = true;
                        break;
                    case 2:
                        __Players__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<string>>().Deserialize(ref reader, formatterResolver);
                        __Players__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.ScoreboardGame(__InternalId__, __TimeElapsed__, __Players__);

            return ____result;
        }
    }


    public sealed class ScoreboardFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.Scoreboard>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public ScoreboardFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Timestamp"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ServerVersion"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Rounds"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ServerConfig"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Scores"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GamesInProgress"), 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Timestamp"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ServerVersion"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Rounds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ServerConfig"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Scores"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GamesInProgress"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.Scoreboard value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.Timestamp, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteString(value.ServerVersion);
            writer.WriteRaw(this.____stringByteKeys[2]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Serialize(ref writer, value.Rounds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::Server.Config.JsonObjects.CurrentConfig>().Serialize(ref writer, value.ServerConfig, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardUser>>().Serialize(ref writer, value.Scores, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardGame>>().Serialize(ref writer, value.GamesInProgress, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.Scoreboard Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __Timestamp__ = default(global::System.DateTime);
            var __Timestamp__b__ = false;
            var __ServerVersion__ = default(string);
            var __ServerVersion__b__ = false;
            var __Rounds__ = default(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>);
            var __Rounds__b__ = false;
            var __ServerConfig__ = default(global::Server.Config.JsonObjects.CurrentConfig);
            var __ServerConfig__b__ = false;
            var __Scores__ = default(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardUser>);
            var __Scores__b__ = false;
            var __GamesInProgress__ = default(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardGame>);
            var __GamesInProgress__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Timestamp__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __Timestamp__b__ = true;
                        break;
                    case 1:
                        __ServerVersion__ = reader.ReadString();
                        __ServerVersion__b__ = true;
                        break;
                    case 2:
                        __Rounds__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Deserialize(ref reader, formatterResolver);
                        __Rounds__b__ = true;
                        break;
                    case 3:
                        __ServerConfig__ = formatterResolver.GetFormatterWithVerify<global::Server.Config.JsonObjects.CurrentConfig>().Deserialize(ref reader, formatterResolver);
                        __ServerConfig__b__ = true;
                        break;
                    case 4:
                        __Scores__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardUser>>().Deserialize(ref reader, formatterResolver);
                        __Scores__b__ = true;
                        break;
                    case 5:
                        __GamesInProgress__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.ScoreboardGame>>().Deserialize(ref reader, formatterResolver);
                        __GamesInProgress__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.Scoreboard(__Timestamp__, __ServerVersion__, __Rounds__, __ServerConfig__, __Scores__, __GamesInProgress__);

            return ____result;
        }
    }


    public sealed class PrimaryConfigFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.PrimaryConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public PrimaryConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ListeningIp"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("ListeningPort"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("MinimumPlayersAmount"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlayersLimit"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameDelay"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Rounds"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CurrentRound"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("NextGameId"), 7},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("WebRootPath"), 8},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LiveView"), 9},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("VerboseView"), 10},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("UsersFileVersion"), 11},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ListeningIp"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ListeningPort"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("MinimumPlayersAmount"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlayersLimit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameDelay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Rounds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CurrentRound"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("NextGameId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("WebRootPath"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LiveView"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("VerboseView"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("UsersFileVersion"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.PrimaryConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ListeningIp);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt16(value.ListeningPort);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteUInt16(value.MinimumPlayersAmount);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteUInt16(value.PlayersLimit);
            writer.WriteRaw(this.____stringByteKeys[4]);
            writer.WriteUInt16(value.GameDelay);
            writer.WriteRaw(this.____stringByteKeys[5]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Serialize(ref writer, value.Rounds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteString(value.CurrentRound);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteUInt32(value.NextGameId);
            writer.WriteRaw(this.____stringByteKeys[8]);
            writer.WriteString(value.WebRootPath);
            writer.WriteRaw(this.____stringByteKeys[9]);
            writer.WriteBoolean(value.LiveView);
            writer.WriteRaw(this.____stringByteKeys[10]);
            writer.WriteBoolean(value.VerboseView);
            writer.WriteRaw(this.____stringByteKeys[11]);
            writer.WriteInt32(value.UsersFileVersion);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.PrimaryConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __ListeningIp__ = default(string);
            var __ListeningIp__b__ = false;
            var __ListeningPort__ = default(ushort);
            var __ListeningPort__b__ = false;
            var __MinimumPlayersAmount__ = default(ushort);
            var __MinimumPlayersAmount__b__ = false;
            var __PlayersLimit__ = default(ushort);
            var __PlayersLimit__b__ = false;
            var __GameDelay__ = default(ushort);
            var __GameDelay__b__ = false;
            var __Rounds__ = default(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>);
            var __Rounds__b__ = false;
            var __CurrentRound__ = default(string);
            var __CurrentRound__b__ = false;
            var __NextGameId__ = default(uint);
            var __NextGameId__b__ = false;
            var __WebRootPath__ = default(string);
            var __WebRootPath__b__ = false;
            var __LiveView__ = default(bool);
            var __LiveView__b__ = false;
            var __VerboseView__ = default(bool);
            var __VerboseView__b__ = false;
            var __UsersFileVersion__ = default(int);
            var __UsersFileVersion__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __ListeningIp__ = reader.ReadString();
                        __ListeningIp__b__ = true;
                        break;
                    case 1:
                        __ListeningPort__ = reader.ReadUInt16();
                        __ListeningPort__b__ = true;
                        break;
                    case 2:
                        __MinimumPlayersAmount__ = reader.ReadUInt16();
                        __MinimumPlayersAmount__b__ = true;
                        break;
                    case 3:
                        __PlayersLimit__ = reader.ReadUInt16();
                        __PlayersLimit__b__ = true;
                        break;
                    case 4:
                        __GameDelay__ = reader.ReadUInt16();
                        __GameDelay__b__ = true;
                        break;
                    case 5:
                        __Rounds__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Deserialize(ref reader, formatterResolver);
                        __Rounds__b__ = true;
                        break;
                    case 6:
                        __CurrentRound__ = reader.ReadString();
                        __CurrentRound__b__ = true;
                        break;
                    case 7:
                        __NextGameId__ = reader.ReadUInt32();
                        __NextGameId__b__ = true;
                        break;
                    case 8:
                        __WebRootPath__ = reader.ReadString();
                        __WebRootPath__b__ = true;
                        break;
                    case 9:
                        __LiveView__ = reader.ReadBoolean();
                        __LiveView__b__ = true;
                        break;
                    case 10:
                        __VerboseView__ = reader.ReadBoolean();
                        __VerboseView__b__ = true;
                        break;
                    case 11:
                        __UsersFileVersion__ = reader.ReadInt32();
                        __UsersFileVersion__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.PrimaryConfig(__ListeningIp__, __ListeningPort__, __MinimumPlayersAmount__, __PlayersLimit__, __GameDelay__, __Rounds__, __CurrentRound__, __NextGameId__, __WebRootPath__, __LiveView__, __VerboseView__, __UsersFileVersion__);
            if(__ListeningIp__b__) ____result.ListeningIp = __ListeningIp__;
            if(__ListeningPort__b__) ____result.ListeningPort = __ListeningPort__;
            if(__MinimumPlayersAmount__b__) ____result.MinimumPlayersAmount = __MinimumPlayersAmount__;
            if(__PlayersLimit__b__) ____result.PlayersLimit = __PlayersLimit__;
            if(__GameDelay__b__) ____result.GameDelay = __GameDelay__;
            if(__Rounds__b__) ____result.Rounds = __Rounds__;
            if(__CurrentRound__b__) ____result.CurrentRound = __CurrentRound__;
            if(__NextGameId__b__) ____result.NextGameId = __NextGameId__;
            if(__WebRootPath__b__) ____result.WebRootPath = __WebRootPath__;
            if(__LiveView__b__) ____result.LiveView = __LiveView__;
            if(__VerboseView__b__) ____result.VerboseView = __VerboseView__;
            if(__UsersFileVersion__b__) ____result.UsersFileVersion = __UsersFileVersion__;

            return ____result;
        }
    }


    public sealed class UserFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.User>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Password"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Score"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Suspended"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LastLogin"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Password"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Score"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Suspended"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LastLogin"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.User value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Password);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>>().Serialize(ref writer, value.Score, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.Suspended);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.LastLogin, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.User Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Password__ = default(string);
            var __Password__b__ = false;
            var __Score__ = default(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>);
            var __Score__b__ = false;
            var __Suspended__ = default(bool);
            var __Suspended__b__ = false;
            var __LastLogin__ = default(global::System.DateTime);
            var __LastLogin__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Password__ = reader.ReadString();
                        __Password__b__ = true;
                        break;
                    case 1:
                        __Score__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.UserRound>>().Deserialize(ref reader, formatterResolver);
                        __Score__b__ = true;
                        break;
                    case 2:
                        __Suspended__ = reader.ReadBoolean();
                        __Suspended__b__ = true;
                        break;
                    case 3:
                        __LastLogin__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __LastLogin__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.User(__Password__, __Score__, __Suspended__, __LastLogin__);
            if(__Password__b__) ____result.Password = __Password__;
            if(__Score__b__) ____result.Score = __Score__;
            if(__Suspended__b__) ____result.Suspended = __Suspended__;
            if(__LastLogin__b__) ____result.LastLogin = __LastLogin__;

            return ____result;
        }
    }


    public sealed class UsersConfigFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.UsersConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UsersConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Users"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Users"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.UsersConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.User>>().Serialize(ref writer, value.Users, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.UsersConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __Users__ = default(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.User>);
            var __Users__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Users__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.User>>().Deserialize(ref reader, formatterResolver);
                        __Users__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.UsersConfig(__Users__);

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 219
#pragma warning disable 168

namespace Utf8Json.Formatters.Server.Config.JsonObjects.OldJsonObjects
{
    using System;
    using Utf8Json;


    public sealed class UserFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.OldJsonObjects.User>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public UserFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Password"), 0},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Score"), 1},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Suspended"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("LastLogin"), 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Password"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Score"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Suspended"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("LastLogin"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.OldJsonObjects.User value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.Password);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Serialize(ref writer, value.Score, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteBoolean(value.Suspended);
            writer.WriteRaw(this.____stringByteKeys[3]);
            formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Serialize(ref writer, value.LastLogin, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.OldJsonObjects.User Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                return null;
            }
            

            var __Password__ = default(string);
            var __Password__b__ = false;
            var __Score__ = default(global::System.Collections.Generic.Dictionary<string, int>);
            var __Score__b__ = false;
            var __Suspended__ = default(bool);
            var __Suspended__b__ = false;
            var __LastLogin__ = default(global::System.DateTime);
            var __LastLogin__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Password__ = reader.ReadString();
                        __Password__b__ = true;
                        break;
                    case 1:
                        __Score__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Deserialize(ref reader, formatterResolver);
                        __Score__b__ = true;
                        break;
                    case 2:
                        __Suspended__ = reader.ReadBoolean();
                        __Suspended__b__ = true;
                        break;
                    case 3:
                        __LastLogin__ = formatterResolver.GetFormatterWithVerify<global::System.DateTime>().Deserialize(ref reader, formatterResolver);
                        __LastLogin__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.OldJsonObjects.User(__Password__, __Score__, __Suspended__, __LastLogin__);
            if(__Password__b__) ____result.Password = __Password__;
            if(__Score__b__) ____result.Score = __Score__;
            if(__Suspended__b__) ____result.Suspended = __Suspended__;
            if(__LastLogin__b__) ____result.LastLogin = __LastLogin__;

            return ____result;
        }
    }


    public sealed class OldUsersConfigFormatter : global::Utf8Json.IJsonFormatter<global::Server.Config.JsonObjects.OldJsonObjects.OldUsersConfig>
    {
        readonly global::Utf8Json.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OldUsersConfigFormatter()
        {
            this.____keyMapping = new global::Utf8Json.Internal.AutomataDictionary()
            {
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Users"), 0},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("Users"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.OldJsonObjects.OldUsersConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.OldJsonObjects.User>>().Serialize(ref writer, value.Users, formatterResolver);
            
            writer.WriteEndObject();
        }

        public global::Server.Config.JsonObjects.OldJsonObjects.OldUsersConfig Deserialize(ref JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            if (reader.ReadIsNull())
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }
            

            var __Users__ = default(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.OldJsonObjects.User>);
            var __Users__b__ = false;

            var ____count = 0;
            reader.ReadIsBeginObjectWithVerify();
            while (!reader.ReadIsEndObjectWithSkipValueSeparator(ref ____count))
            {
                var stringKey = reader.ReadPropertyNameSegmentRaw();
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    reader.ReadNextBlock();
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Users__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.OldJsonObjects.User>>().Deserialize(ref reader, formatterResolver);
                        __Users__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.OldJsonObjects.OldUsersConfig(__Users__);

            return ____result;
        }
    }

}

#pragma warning disable 168
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
