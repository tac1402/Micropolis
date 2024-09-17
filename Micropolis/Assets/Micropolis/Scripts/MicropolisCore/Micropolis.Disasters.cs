using UnityEngine;
using System;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Let disasters happen.
        /// TODO Decide what to do with the 'nothing happens' disaster (since the
        ///      chance that a disaster happens is expressed in the DisChance
        ///      table).
        /// </summary>
        public void doDisasters()
        {
            // Chance of disasters at lev 0 1 2
            short[] DisChance =
            {
                10 * 48,    // Game level 0
                5 * 48,     // Game level 1
                60          // Game level 2
            };
            // assert(LEVEL_COUNT == LENGTH_OF(DisChance));

            if (floodCount != 0)
            {
                floodCount--;
            }

            if (disasterEvent != ScenarioType.SC_NONE)
            {
                scenarioDisaster();
            }

            if (!enableDisasters) // Disasters have been disabled
            {
                return;
            }

            int x = (int) gameLevel;
            if (x > (int) GameLevel.LEVEL_LAST)
            {
                x = (int) GameLevel.LEVEL_EASY;
            }

            if (getRandom(DisChance[x]) == 0)
            {
                switch (getRandom(8))
                {
                    case 0:
                    case 1:
                        setFire(); // 2/9 chance a fire breaks out
                        break;

                    case 2:
                    case 3:
                        makeFlood(); // 2/9 chance for a flood
                        break;

                    case 4:
                        makeAirCrash(); // 1/9 chance for an airplane crash
                        break;

                    case 5:
                        makeTornado(); // 1/9 chance tornado
                        break;

                    case 6:
                        makeEarthquake(); // 1/9 chance earthquake
                        break;

                    case 7:
                    case 8:
                        // 2/9 chance a scary monster arrives in a dirty town
                        if (pollutionAverage > /* 80 */ 60)
                        {
                            makeMonster();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Let disasters of the scenario happen.
        /// </summary>
        public void scenarioDisaster()
        {
            switch (disasterEvent)
            {
                case ScenarioType.SC_DULLSVILLE:
                    break;

                case ScenarioType.SC_SAN_FRANCISCO:
                    if (disasterWait == 1)
                    {
                        makeEarthquake();
                    }
                    break;

                case ScenarioType.SC_HAMBURG:
                    if (disasterWait % 10 == 0)
                    {
                        makeFireBombs();
                    }
                    break;

                case ScenarioType.SC_BERN:
                    break;

                case ScenarioType.SC_TOKYO:
                    if (disasterWait == 1)
                    {
                        makeMonster();
                    }
                    break;

                case ScenarioType.SC_DETROIT:
                    break;

                case ScenarioType.SC_BOSTON:
                    if (disasterWait == 1)
                    {
                        makeMeltdown();
                    }
                    break;

                case ScenarioType.SC_RIO:
                    if (disasterWait % 24 == 0)
                    {
                        makeFlood();
                    }
                    break;
            }

            if (disasterWait > 0)
            {
                disasterWait--;
            }
            else
            {
                disasterEvent = ScenarioType.SC_NONE;
            }
        }

        /// <summary>
        /// Crash an airplane
        /// </summary>
        private void makeAirCrash()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Make a nuclear power plant melt down.
        /// TODO Randomize which nuke plant melts down.
        /// </summary>
        public void makeMeltdown()
        {
            short x, y;

            for (x = 0; x < (WORLD_W - 1); x++)
            {
                for (y = 0; y < (WORLD_H - 1); y++)
                {
                    //if ((oldMap[x, y] & (ushort) MapTileBits.LOMASK) == (ushort) MapTileCharacters.NUCLEAR)
                    if (map[new Vector3(x, 0, y)].Id == (ushort)MapTileCharacters.NUCLEAR)
                    {
                        doMeltdown(new Position(x, y));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Perform a nuclear melt-down disaster
        /// </summary>
        /// <param name="pos">Position of the nuclear power plant that melts.</param>
        private void doMeltdown(Position pos)
        {
            makeExplosion(pos.posX - 1, pos.posY - 1);
            makeExplosion(pos.posX - 1, pos.posY + 2);
            makeExplosion(pos.posX + 2, pos.posY - 1);
            makeExplosion(pos.posX + 2, pos.posY + 2);

            // Whole power plant is at fire
            for (int x = pos.posX - 1; x < pos.posX + 3; x++)
            {
                for (int y = pos.posY - 1; y < pos.posY + 3; y++)
                {
                    //oldMap[x,y] = randomFire();
                    map[new Vector3(x, 0, y)].Id = randomFire();
				}
            }

            // Add lots of radiation tiles around the plant
            for (int z = 0; z < 200; z++)
            {

                int x = pos.posX - 20 + getRandom(40);
                int y = pos.posY - 15 + getRandom(30);

                if (!Position.testBounds((short) x, (short) y))
                { 
                    // Ignore off-map positions
                    continue;
                }

                //ushort t = oldMap[x,y];
				TileInfo t = map[new Vector3(x, 0, y)];

				if (t.IsCenter == true)
                {
                    continue; // Ignore zones
                }

                if (t.CanLit == true || t.Id == (ushort) MapTileCharacters.DIRT)
                {
                    //oldMap[x,y] = (ushort) MapTileCharacters.RADTILE; // Make tile radio-active
                    map[new Vector3(x, 0, y)].Id = (ushort)MapTileCharacters.RADTILE; // Make tile radio-active
				}

            }

            // Report disaster to the user
            sendMessage((short) MessageNumber.MESSAGE_NUCLEAR_MELTDOWN, (short) pos.posX, (short) pos.posY, true, true);
        }

        /// <summary>
        /// Let a fire bomb explode at a random location.
        /// </summary>
        public void fireBomb()
        {
            int crashX = getRandom(WORLD_W - 1);
            int crashY = getRandom(WORLD_H - 1);
            makeExplosion(crashX, crashY);
            sendMessage((short) MessageNumber.MESSAGE_FIREBOMBING, (short) crashX, (short) crashY, true, true);
        }

        /// <summary>
        /// Throw several bombs onto the city.
        /// </summary>
        public void makeFireBombs()
        {
            int count = 2 + (getRandom16() & 1);

            while (count > 0)
            {
                fireBomb();
                count--;
            }

            // TODO: Schedule periodic fire bombs over time, every few ticks.
        }

        /// <summary>
        /// Change random tiles to fire or dirt as result of the earthquake.
        /// </summary>
        public void makeEarthquake()
        {
            short x, y, z;

            int strength = getRandom(700) + 300; // strength/duration of the earthquake

            doEarthquake(strength);

            sendMessage((short) MessageNumber.MESSAGE_EARTHQUAKE, cityCenterX, cityCenterY, true);

            for (z = 0; z < strength; z++)
            {
                x = getRandom(WORLD_W - 1);
                y = getRandom(WORLD_H - 1);

                //if (vulnerable(oldMap[x,y]))
                if (vulnerable(map[new Vector3(x, 0, y)]))
                {
                    if ((z & 0x3) != 0)
                    { 
                        // 3 of 4 times reduce to rubble
                        //oldMap[x,y] = randomRubble();
						map[new Vector3(x, 0, y)].Id = randomRubble();
						map[new Vector3(x, 0, y)].IsBulldozable = true;
					}
					else
                    {
                        // 1 of 4 times start fire
                        //oldMap[x,y] = randomFire();
						map[new Vector3(x, 0, y)].Id = randomFire();

					}
                }
            }
        }

        /// <summary>
        /// Start a fire at a random place, random disaster or scenario.
        /// </summary>
        public void setFire()
        {
            short x, y;

            x = getRandom(WORLD_W - 1);
            y = getRandom(WORLD_H - 1);
            //z = (short) oldMap[x,y];
            TileInfo t = map[new Vector3(x, 0, y)];

            if (t.IsCenter == false)
            {
                //z = (short)(z & (ushort) MapTileBits.LOMASK);
                if (t.Id > (ushort) MapTileCharacters.LHTHR && t.Id < (ushort) MapTileCharacters.LASTZONE)
                {
                    //oldMap[x,y] = randomFire();
					map[new Vector3(x, 0, y)].Id = randomFire();
					sendMessage((short) MessageNumber.MESSAGE_FIRE_REPORTED, x, y, true);
                }
            }
        }

        /// <summary>
        /// Start a fire at a random place, requested by user.
        /// </summary>
        /*public void makeFire()
        {
            short t, x, y, z;

            for (t = 0; t < 40; t++)
            {
                x = getRandom(WORLD_W - 1);
                y = getRandom(WORLD_H - 1);
                z = (short) oldMap[x,y];

                if ((z & (short) MapTileBits.IsCenter) == 0 && (z & (ushort) MapTileBits.CanLit) != 0)
                {
                    z = (short)(z & (ushort) MapTileBits.LOMASK);
                    if ((z > 21) && (z < (ushort) MapTileCharacters.LASTZONE))
                    {
                        //oldMap[x,y] = randomFire();
						map[new Vector3(x, 0, y)].Id = randomFire();
						sendMessage((short) MessageNumber.MESSAGE_FIRE_REPORTED, x, y);
                        return;
                    }
                }
            }
        }*/

        /// <summary>
        /// Is tile vulnerable for an earthquake?
        /// </summary>
        /// <param name="tem">Tile data</param>
        /// <returns>Function returns true if tile is vulnerable, and false if not</returns>
        public bool vulnerable(TileInfo tileInfo)
        {
            //int tem2 = tem & (ushort) MapTileBits.LOMASK;

            if (tileInfo.Id < (int) MapTileCharacters.RESBASE || tileInfo.Id > (int) MapTileCharacters.LASTZONE || tileInfo.IsCenter == true)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Flood many tiles.
        /// TODO Use Direction and some form of XYPosition class here.
        /// </summary>
        public void makeFlood()
        {
            short[] Dx = { 0, 1, 0, -1 };
            short[] Dy = { -1, 0, 1, 0 };
            short xx, yy;
            short z, t, x, y;

            for (z = 0; z < 300; z++)
            {
                x = getRandom(WORLD_W - 1);
                y = getRandom(WORLD_H - 1);
                //c = (short)(oldMap[x,y] & (ushort) MapTileBits.LOMASK);
				TileInfo c = map[new Vector3(x, 0, y)];

				if (c.Id > (short) MapTileCharacters.CHANNEL && c.Id <= (short) MapTileCharacters.WATER_HIGH)
                { 
                    /* if riveredge  */
                    for (t = 0; t < 4; t++)
                    {
                        xx = (short) (x + Dx[t]);
                        yy = (short) (y + Dy[t]);
                        if (Position.testBounds(xx, yy))
                        {
                            //c = (short) oldMap[xx,yy];

                            /* tile is floodable */
                            if (c.Id == (short) MapTileCharacters.DIRT || c.IsBulldozable == true || c.CanLit == true)
                                /*|| (c & ((short) MapTileBits.IsBulldozable | (short) MapTileBits.CanLit)) == ((short) MapTileBits.IsBulldozable | (short) MapTileBits.CanLit))*/
                            {
                                //oldMap[xx,yy] = (ushort) MapTileCharacters.FLOOD;
								map[new Vector3(xx, 0, yy)].Id = (ushort)MapTileCharacters.FLOOD;
								floodCount = 30;
                                sendMessage((short) MessageNumber.MESSAGE_FLOODING_REPORTED, xx, yy, true);
                                return;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Flood around the given position.
        /// TODO Use some form of rotating around a position.
        /// </summary>
        /// <param name="pos">Position around which to flood further</param>
        public void doFlood(Position pos)
        {
            short[] Dx = { 0, 1, 0, -1 };
            short[] Dy = { -1, 0, 1, 0 };

            if (floodCount > 0)
            {
                // Flood is not over yet
                for (int z = 0; z < 4; z++)
                {
                    if ((getRandom16() & 7) == 0)
                    { 
                        // 12.5% chance
                        int xx = pos.posX + Dx[z];
                        int yy = pos.posY + Dy[z];
                        if (Position.testBounds((short) xx, (short) yy))
                        {
                            //ushort c = oldMap[xx,yy];
                            //ushort t = (ushort)(c & (ushort) MapTileBits.LOMASK);

                            TileInfo t = map[new Vector3(xx, 0, yy)];

                            if (t.CanLit == true || t.Id == (ushort) MapTileCharacters.DIRT
                                || (t.Id >= (ushort) MapTileCharacters.WOODS5 && t.Id < (ushort) MapTileCharacters.FLOOD))
                            {
                                if (t.IsCenter == true)
                                {
                                    fireZone(new Position(xx, yy), t);
                                }
                                //oldMap[xx,yy] = (ushort) (MapTileCharacters.FLOOD + getRandom(2));
								map[new Vector3(xx, 0, yy)].Id = (ushort)(MapTileCharacters.FLOOD + getRandom(2));
							}
						}
                    }
                }
            }
            else
            {
                if ((getRandom16() & 15) == 0)
                { 
                    // 1/16 chance
                    //oldMap[pos.posX,pos.posY] = (ushort) MapTileCharacters.DIRT;
                    map[new Vector3(pos.posX,0, pos.posY)].Id = (ushort)MapTileCharacters.DIRT;
				}
            }
        }
    }
}
