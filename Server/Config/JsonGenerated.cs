﻿#pragma warning disable 618
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
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(7)
            {
                {typeof(global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>), 0 },
                {typeof(global::System.Collections.Generic.Dictionary<string, int>), 1 },
                {typeof(global::System.Collections.Generic.Dictionary<string, global::Server.Config.JsonObjects.User>), 2 },
                {typeof(global::Server.Config.JsonObjects.Round), 3 },
                {typeof(global::Server.Config.JsonObjects.PrimaryConfig), 4 },
                {typeof(global::Server.Config.JsonObjects.User), 5 },
                {typeof(global::Server.Config.JsonObjects.UsersConfig), 6 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::Utf8Json.Formatters.ListFormatter<global::Server.Config.JsonObjects.Round>();
                case 1: return new global::Utf8Json.Formatters.DictionaryFormatter<string, int>();
                case 2: return new global::Utf8Json.Formatters.DictionaryFormatter<string, global::Server.Config.JsonObjects.User>();
                case 3: return new Utf8Json.Formatters.Server.Config.JsonObjects.RoundFormatter();
                case 4: return new Utf8Json.Formatters.Server.Config.JsonObjects.PrimaryConfigFormatter();
                case 5: return new Utf8Json.Formatters.Server.Config.JsonObjects.UserFormatter();
                case 6: return new Utf8Json.Formatters.Server.Config.JsonObjects.UsersConfigFormatter();
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
            if(__ShortName__b__) ____result.ShortName = __ShortName__;
            if(__Name__b__) ____result.Name = __Name__;
            if(__DisplayOrder__b__) ____result.DisplayOrder = __DisplayOrder__;

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
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("PlayersLimit"), 2},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("GameDelay"), 3},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("Rounds"), 4},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("CurrentRound"), 5},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("NextGameId"), 6},
                { JsonWriter.GetEncodedPropertyNameWithoutQuotation("WebRootPath"), 7},
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonWriter.GetEncodedPropertyNameWithBeginObject("ListeningIp"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("ListeningPort"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("PlayersLimit"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("GameDelay"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("Rounds"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("CurrentRound"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("NextGameId"),
                JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("WebRootPath"),
                
            };
        }

        public void Serialize(ref JsonWriter writer, global::Server.Config.JsonObjects.PrimaryConfig value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
        {
            

            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.WriteString(value.ListeningIp);
            writer.WriteRaw(this.____stringByteKeys[1]);
            writer.WriteUInt16(value.ListeningPort);
            writer.WriteRaw(this.____stringByteKeys[2]);
            writer.WriteUInt16(value.PlayersLimit);
            writer.WriteRaw(this.____stringByteKeys[3]);
            writer.WriteUInt16(value.GameDelay);
            writer.WriteRaw(this.____stringByteKeys[4]);
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Serialize(ref writer, value.Rounds, formatterResolver);
            writer.WriteRaw(this.____stringByteKeys[5]);
            writer.WriteString(value.CurrentRound);
            writer.WriteRaw(this.____stringByteKeys[6]);
            writer.WriteUInt32(value.NextGameId);
            writer.WriteRaw(this.____stringByteKeys[7]);
            writer.WriteString(value.WebRootPath);
            
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
                        __PlayersLimit__ = reader.ReadUInt16();
                        __PlayersLimit__b__ = true;
                        break;
                    case 3:
                        __GameDelay__ = reader.ReadUInt16();
                        __GameDelay__b__ = true;
                        break;
                    case 4:
                        __Rounds__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::Server.Config.JsonObjects.Round>>().Deserialize(ref reader, formatterResolver);
                        __Rounds__b__ = true;
                        break;
                    case 5:
                        __CurrentRound__ = reader.ReadString();
                        __CurrentRound__b__ = true;
                        break;
                    case 6:
                        __NextGameId__ = reader.ReadUInt32();
                        __NextGameId__b__ = true;
                        break;
                    case 7:
                        __WebRootPath__ = reader.ReadString();
                        __WebRootPath__b__ = true;
                        break;
                    default:
                        reader.ReadNextBlock();
                        break;
                }

                NEXT_LOOP:
                continue;
            }

            var ____result = new global::Server.Config.JsonObjects.PrimaryConfig(__ListeningIp__, __ListeningPort__, __PlayersLimit__, __GameDelay__, __Rounds__, __CurrentRound__, __NextGameId__, __WebRootPath__);
            if(__ListeningIp__b__) ____result.ListeningIp = __ListeningIp__;
            if(__ListeningPort__b__) ____result.ListeningPort = __ListeningPort__;
            if(__PlayersLimit__b__) ____result.PlayersLimit = __PlayersLimit__;
            if(__GameDelay__b__) ____result.GameDelay = __GameDelay__;
            if(__Rounds__b__) ____result.Rounds = __Rounds__;
            if(__CurrentRound__b__) ____result.CurrentRound = __CurrentRound__;
            if(__NextGameId__b__) ____result.NextGameId = __NextGameId__;
            if(__WebRootPath__b__) ____result.WebRootPath = __WebRootPath__;

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
            formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<string, int>>().Serialize(ref writer, value.Score, formatterResolver);
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
