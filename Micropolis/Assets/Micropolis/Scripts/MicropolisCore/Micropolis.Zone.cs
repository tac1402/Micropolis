using UnityEngine;
using System;
using UnityEngine.UIElements;

namespace MicropolisCore
{
    public partial class Micropolis
    {
        /// <summary>
        /// Handle zone
        /// TODO make a new Zone class then a CommericalZone, IndustrialZone, and ResidentialZone from it
        /// </summary>
        /// <param name="pos">Position of the zone.</param>
        private void doZone(Position pos)
        {
            // Set Power Bit in Map from powerGridMap
            //bool zonePowerFlag = setZonePower(pos);

            /*if (zonePowerFlag)
            {
                poweredZoneCount++;
            }
            else
            {
                unpoweredZoneCount++;
            }*/

            bool zonePowerFlag = false;

			//ushort tile = (ushort)(oldMap[pos.posX,pos.posY] & (ushort) MapTileBits.LOMASK);
			int tile = map[new Vector3(pos.posX, 0, pos.posY)].Id;

			// Do special zones.
			if (tile > (ushort) MapTileCharacters.PORTBASE)
            {
                doSpecialZone(pos, zonePowerFlag);
                return;
            }

            // Do residential zones.
            if (tile < (ushort)MapTileCharacters.HOSPITAL)
            {
                doResidential(pos, zonePowerFlag);
                return;
            }

            // Do hospitals and churches.
            if (tile < (ushort)MapTileCharacters.COMBASE)
            {
                doHospitalChurch(pos);
                return;
            }

            // Do commercial zones.
            if (tile < (ushort)MapTileCharacters.INDBASE)
            {
                doCommercial(pos, zonePowerFlag);
                return;
            }

            // Do industrial zones.
            doIndustrial(pos, zonePowerFlag);
        }

        /// <summary>
        /// Handle industrial zone.
        /// </summary>
        /// <param name="pos">Position of the industrial zone.</param>
        /// <param name="zonePower">Does the zone have power?</param>
        private void doIndustrial(Position pos, bool zonePower)
        {
            short tpop, zscore, TrfGood;



            Vector3 center = new Vector3(pos.posX, 0, pos.posY);
			TileInfo tile = map[center];

			City.InitZone(Zone.Industrial, tile, center);

			if (tile != null)
			{
				Industrial industrial = tile.Tile.GetComponent<Industrial>();
				zonePower = industrial.Processor.IsElectro;
			}

			indZonePop++;
            //setSmoke(pos, zonePower);
            tpop = getIndZonePop((ushort)tile.Id);
            indPop += tpop;

            if (tpop > getRandom(5))
            {
                /* Try driving from industrial to residential */
                TrfGood = makeTraffic(pos, ZoneType.ZT_RESIDENTIAL);
            }
            else
            {
                TrfGood = 1;
            }

            if (TrfGood == -1)
            {
                doIndOut(pos, tpop, getRandom16() & 1);
                return;
            }

            if ((getRandom16() & 7) == 0)
            {
                zscore = (short) (indValve + evalInd(TrfGood));

                if (!zonePower)
                {
                    zscore = -500;
                }

                if (zscore > -350 &&
                    (((short)(zscore - 26380)) > ((short)getRandom16Signed())))
                {
                    doIndIn(pos, tpop, getRandom16() & 1);
                    return;
                }

                if (zscore < 350 &&
                    (((short)(zscore + 26380)) < ((short)getRandom16Signed())))
                {
                    doIndOut(pos, tpop, getRandom16() & 1);
                }
            }
        }

        /// <summary>
        /// Handle immigration of industrial zone.
        /// </summary>
        /// <param name="pos">Position of the center tile of the industrial tile.</param>
        /// <param name="pop">Population value of the industrial zone.</param>
        /// <param name="value">Random land value (it seems).</param>
        private void doIndIn(Position pos, int pop, int value)
        {
            if (pop < 4)
            {
                indPlop(pos, pop, value);
                incRateOfGrowth(pos, 8);
            }
        }

        /// <summary>
        /// Place an industrial zone around center tile pos.
        /// </summary>
        /// <param name="pos">Center of the industrial zone.</param>
        /// <param name="den">Population density of the industrial zone (0, 1, 2, or 3).</param>
        /// <param name="value">Landvalue of the industrial zone (0 or 1).</param>
        private void indPlop(Position pos, int den, int value)
        {
            short baseValue = (short) (((value * 4) + den) * 9 + (short) MapTileCharacters.IND1);
            zonePlop(pos, baseValue);
        }

        /// <summary>
        /// Compute evaluation of an industrial zone.
        /// </summary>
        /// <param name="traf">Result if traffic attempt.</param>
        /// <returns>Evaluation value of the industrial zone.</returns>
        private short evalInd(int traf)
        {
            if (traf < 0)
            {
                return -1000;
            }

            return 0;
        }

        /// <summary>
        /// Handle industrial zone emigration.
        /// </summary>
        /// <param name="pos">Position of the center tile of the industrial tile.</param>
        /// <param name="pop">Population value of the industrial zone.</param>
        /// <param name="value">Random land value (it seems).</param>
        private void doIndOut(Position pos, int pop, int value)
        {
            if (pop > 1)
            {
                indPlop(pos, pop - 2, value);
                incRateOfGrowth(pos, -8);
                return;
            }

            if (pop == 1)
            {
                zonePlop(pos, (short) MapTileCharacters.INDBASE); // empty industrial zone
                incRateOfGrowth(pos, -8);
            }
        }

        private void setSmoke(Position pos, bool zonePower)
        {
            bool[] aniThis = { true, false, true, true, false, false, true, true };
            short[] dx1 = { -1, 0, 1, 0, 0, 0, 0, 1 };
            short[] dy1 = { -1, 0, -1, -1, 0, 0, -1, -1 };
            short[] aniTabA = { 0, 0, 32, 40, 0, 0, 48, 56 };
            short[] aniTabB = { 0, 0, 36, 44, 0, 0, 52, 60 };
            short[] aniTabC = { (short)MapTileCharacters.IND1, 0, (short)MapTileCharacters.IND2, (short)MapTileCharacters.IND4, 0, 0, (short)MapTileCharacters.IND6, (short)MapTileCharacters.IND8 };
            short[] aniTabD = { (short)MapTileCharacters.IND1, 0, (short)MapTileCharacters.IND3, (short)MapTileCharacters.IND5, 0, 0, (short)MapTileCharacters.IND7, (short)MapTileCharacters.IND9 };

            //ushort tile = (ushort)(oldMap[pos.posX, pos.posY] & (ushort) MapTileBits.LOMASK);
			int tile = map[new Vector3(pos.posX, 0, pos.posY)].Id;

			int z = (tile - (ushort) MapTileCharacters.IZB) >> 3;
            z = z & 7;

            if (aniThis[z])
            {
                int xx = pos.posX + dx1[z];
                int yy = pos.posY + dy1[z];

                if (Position.testBounds((short) xx, (short) yy))
                {
					Vector3 position = new Vector3(xx, 0, yy);
					if (zonePower)
                    {
                        // TODO Why do we assign the same map position twice?
                        // TODO Add SMOKEBASE into aniTabA and aniTabB tables?
						if (map[position].Id == aniTabC[z])
						{
							//oldMap[xx, yy] = (ushort)((ushort)MapTileBits.ASCBIT | ((ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabA[z]));
							//oldMap[xx, yy] = (ushort)((ushort)MapTileBits.ASCBIT | ((ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabB[z]));
                            map[position].Id = (ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabA[z];
							map[position].CanConduct = true;
							map[position].CanLit = true;
						}
					}
					else
                    {
                        // TODO Why do we assign the same map position twice?
                        if (map[position].Id == aniTabC[z])
                        {
                            //oldMap[xx, yy] = (ushort)((ushort)MapTileBits.REGBIT | ((ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabC[z]));
                            //oldMap[xx, yy] = (ushort)((ushort)MapTileBits.REGBIT | ((ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabD[z]));

							map[position].Id = (ushort)MapTileCharacters.SMOKEBASE + (ushort)aniTabB[z];
							map[position].CanConduct = true;
							map[position].CanLit = true;
						}
					}
                }
            }
        }

        /// <summary>
        /// Handle commercial zone.
        /// </summary>
        /// <param name="pos">Position of the commercial zone.</param>
        /// <param name="zonePower">Does the zone have power?</param>
        private void doCommercial(Position pos, bool zonePower)
        {
            short tpop, TrfGood;
            short zscore, locvalve, value;

            //ushort tile = (ushort) (oldMap[pos.posX,pos.posY] & (ushort) MapTileBits.LOMASK);
            Vector3 center = new Vector3(pos.posX, 0, pos.posY);
			TileInfo tile = map[center];

			City.InitZone(Zone.Commercial, tile, center);

			if (tile != null)
			{
				Commercial commercial = tile.Tile.GetComponent<Commercial>();
				zonePower = commercial.Processor.IsElectro;
			}


			comZonePop++;
            tpop = getComZonePop((ushort)tile.Id);
            comPop += tpop;

            if (tpop > getRandom(5))
            {
                /* Try driving from commercial to industrial */
                TrfGood = makeTraffic(pos, ZoneType.ZT_INDUSTRIAL);
            }
            else
            {
                TrfGood = 1;
            }

            if (TrfGood == -1)
            {
                value = getLandPollutionValue(pos);
                doComOut(pos, tpop, value);
                return;
            }

            if ((getRandom16() & 7) == 0)
            {

                locvalve = evalCom(pos, TrfGood);
                zscore = (short) (comValve + locvalve);

                if (!zonePower)
                {
                    zscore = -500;
                }

                if (TrfGood != 0 &&
                    zscore > -350 &&
                    (short)(zscore - 26380) > (short)getRandom16Signed())
                {
                    value = getLandPollutionValue(pos);
                    doComIn(pos, tpop, value);
                    return;
                }

                if ((zscore < 350) &&
                    (((short)(zscore + 26380)) < ((short)getRandom16Signed())))
                {
                    value = getLandPollutionValue(pos);
                    doComOut(pos, tpop, value);
                }

            }
        }

        /// <summary>
        /// Handle immigration of commercial zone.
        /// </summary>
        /// <param name="pos">Position of the commercial zone.</param>
        /// <param name="pop">Population ?</param>
        /// <param name="value">Land value corrected for pollution.</param>
        private void doComIn(Position pos, int pop, int value)
        {
            short z;

            z = landValueMap.worldGet(pos.posX, pos.posY);
            z = (short) (z >> 5);

            if (pop > z)
            {
                return;
            }

            if (pop < 5)
            {
                comPlop(pos, pop, value);
                incRateOfGrowth(pos, 8);
            }
        }

        /// <summary>
        /// Compute evaluation of a commercial zone.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="traf">Result if traffic attempt.</param>
        /// <returns>Evaluation value of the commercial zone.</returns>
        private short evalCom(Position pos, int traf)
        {
            short Value;

            if (traf < 0)
            {
                return -3000;
            }

            Value = comRateMap.worldGet(pos.posX, pos.posY);

            return Value;
        }

        /// <summary>
        /// Handle emigration of commercial zone.
        /// </summary>
        /// <param name="pos">Position of the commercial zone.</param>
        /// <param name="pop">Population ?</param>
        /// <param name="value">Land value corrected for pollution.</param>
        private void doComOut(Position pos, int pop, int value)
        {
            if (pop > 1)
            {
                comPlop(pos, pop - 2, value);
                incRateOfGrowth(pos, -8);
                return;
            }

            if (pop == 1)
            {
                zonePlop(pos, (short) MapTileCharacters.COMBASE);
                incRateOfGrowth(pos, -8);
            }
        }

        /// <summary>
        /// Build a commercial zone.
        /// </summary>
        /// <param name="pos">Position of the commercial zone.</param>
        /// <param name="Den">Density</param>
        /// <param name="Value">Land value corrected for pollution.</param>
        private void comPlop(Position pos, int Den, int Value)
        {
            short baseValue;

            baseValue = (short) ((Value * 5 + Den) * 9 + MapTileCharacters.CZB - 4);
            zonePlop(pos, baseValue);
        }

        /// <summary>
        /// Update special zones.
        /// </summary>
        /// <param name="pos">Position of the zone.</param>
        /// <param name="powerOn">Zone is powered.</param>
        private void doSpecialZone(Position pos, bool powerOn)
        {
            // Bigger numbers reduce chance of nuclear melt down
            short[] meltdownTable = { 30000, 20000, 10000 };

            Vector3 center = new Vector3(pos.posX, 0, pos.posY);
			TileInfo tile = map[center];

			switch (tile.Id)
            {

                case (ushort) MapTileCharacters.POWERPLANT:

					City.InitZone(Zone.CoalPowerPlant, tile, center);

					coalPowerPop++;

                    if ((cityTime & 7) == 0)
                    {
                        repairZone(pos, (ushort) MapTileCharacters.POWERPLANT, 4); /* post */
                    }

                    pushPowerStack(pos);
                    //coalSmoke(pos);

                    return;

                case (ushort)MapTileCharacters.NUCLEAR:

					City.InitZone(Zone.NuclearPowerPlant, tile, center);
					
                    if (enableDisasters && getRandom(meltdownTable[(int) gameLevel]) == 0)
                    {
                        doMeltdown(pos);
                        return;
                    }

                    nuclearPowerPop++;

                    if ((cityTime & 7) == 0)
                    {
                        repairZone(pos, (ushort) MapTileCharacters.NUCLEAR, 4); /* post */
                    }

                    pushPowerStack(pos);

                    return;

                case (ushort)MapTileCharacters.FIRESTATION:
                    {

                        int z;

                        fireStationPop++;

                        if ((cityTime & 7) == 0)
                        {
                            repairZone(pos, (ushort) MapTileCharacters.FIRESTATION, 3); /* post */
                        }

                        if (powerOn)
                        {
                            z = (int) fireEffect;                   /* if powered get effect  */
                        }
                        else
                        {
                            z = (int) (fireEffect / 2);               /* from the funding ratio  */
                        }

                        Position pos2 = new Position(pos);
                        bool foundRoad = findPerimeterRoad(pos2);

                        if (!foundRoad)
                        {
                            z = z / 2;                        /* post FD's need roads  */
                        }

                        int value = fireStationMap.worldGet(pos2.posX, pos2.posY);
                        value += z;
                        fireStationMap.worldSet(pos2.posX, pos2.posY, (short) value);

                        return;
                    }

                case (ushort)MapTileCharacters.POLICESTATION:
                    {

                        int z;

                        policeStationPop++;

                        if ((cityTime & 7) == 0)
                        {
                            repairZone(pos, (ushort) MapTileCharacters.POLICESTATION, 3); /* post */
                        }

                        if (powerOn)
                        {
                            z = (int) policeEffect;
                        }
                        else
                        {
                            z = (int) (policeEffect / 2);
                        }

                        Position pos2 = new Position(pos);
                        bool foundRoad = findPerimeterRoad(pos2);

                        if (!foundRoad)
                        {
                            z = z / 2; /* post PD's need roads */
                        }

                        int value = policeStationMap.worldGet(pos2.posX, pos2.posY);
                        value += z;
                        policeStationMap.worldSet(pos2.posX, pos2.posY, (short) value);

                        return;
                    }

                case (ushort)MapTileCharacters.STADIUM:  // Empty stadium

                    stadiumPop++;

                    if ((cityTime & 15) == 0)
                    {
                        repairZone(pos, (ushort) MapTileCharacters.STADIUM, 4);
                    }

                    if (powerOn)
                    {
                        // Every now and then, display a match
                        if (((cityTime + pos.posX + pos.posY) & 31) == 0)
                        {
                            drawStadium(pos, (ushort) MapTileCharacters.FULLSTADIUM);
                            //map[pos.posX + 1,pos.posY] = (ushort) MapTileCharacters.FOOTBALLGAME1 + (ushort) MapTileBits.ANIMBIT;
                            //map[pos.posX + 1,pos.posY + 1] = (ushort) MapTileCharacters.FOOTBALLGAME2 + (ushort) MapTileBits.ANIMBIT;
                        }
                    }

                    return;

                case (ushort)MapTileCharacters.FULLSTADIUM:  // Full stadium

                    stadiumPop++;

                    if (((cityTime + pos.posX + pos.posY) & 7) == 0)
                    {
                        // Stop the match
                        drawStadium(pos, (ushort) MapTileCharacters.STADIUM);
                    }

                    return;

                case (ushort)MapTileCharacters.AIRPORT:

                    airportPop++;

                    if ((cityTime & 7) == 0)
                    {
                        repairZone(pos, (ushort)MapTileCharacters.AIRPORT, 6);
                    }

                    // If powered, display a rotating radar
                    /*if (powerOn)
                    {
                        if ((ushort)(map[pos.posX + 1,pos.posY - 1] & (ushort) MapTileBits.LOMASK) == 
                            (ushort) MapTileCharacters.RADAR)
                        {
                            map[pos.posX + 1,pos.posY - 1] = (ushort) MapTileCharacters.RADAR0 + (ushort)MapTileBits.ANIMBIT + (ushort)MapTileBits.CONDBIT + (ushort)MapTileBits.BURNBIT;
                        }
                    }
                    else
                    {
                        map[pos.posX + 1,pos.posY - 1] = (ushort)MapTileCharacters.RADAR + (ushort)MapTileBits.CONDBIT + (ushort) MapTileBits.BURNBIT;
                    }*/

                    if (powerOn)
                    { 
                        // Handle the airport only if there is power
                        doAirport(pos);
                    }

                    return;

                case (ushort)MapTileCharacters.PORT:

                    seaportPop++;

                    if ((cityTime & 15) == 0)
                    {
                        repairZone(pos, (ushort)MapTileCharacters.PORT, 4);
                    }

                    // If port has power and there is no ship, generate one
                    if (powerOn && getSprite((int) SpriteType.SPRITE_SHIP) == null)
                    {
                        generateShip();
                    }

                    return;
            }
        }

        /// <summary>
        /// Generate a airplane or helicopter every now and then.
        /// </summary>
        /// <param name="pos">Position of the airport to start from.</param>
        private void doAirport(Position pos)
        {
            if (getRandom(5) == 0)
            {
                generatePlane(pos);
                return;
            }

            if (getRandom(12) == 0)
            {
                generateCopter(pos);
            }
        }

        /// <summary>
        /// Draw a stadium (either full or empty).
        /// </summary>
        /// <param name="center">Center tile position of the stadium.</param>
        /// <param name="z">Base tile value.</param>
        private void drawStadium(Position center, ushort z)
        {
            int x, y;

            z = (ushort) (z - 5);

            for (y = center.posY - 1; y < center.posY + 3; y++)
            {
                for (x = center.posX - 1; x < center.posX + 3; x++)
                {
                    //oldMap[x,y] = (ushort)(z | (ushort) MapTileBits.BNCNBIT);
					map[new Vector3(x, 0, y)].Id = z;
					map[new Vector3(x, 0, y)].CanConduct = true;
                    map[new Vector3(x, 0, y)].CanLit = true;
					z++;
                }
            }

			//oldMap[center.posX, center.posY] |= (ushort)MapTileBits.ZONEBIT | (ushort)MapTileBits.PWRBIT;
			map[new Vector3(center.posX, 0, center.posY)].IsCenter = true;
			map[new Vector3(center.posX, 0, center.posY)].IsPower = true;
        }

        /// <summary>
        /// Draw coal smoke tiles around given position (of a coal power plant).
        /// </summary>
        /// <param name="pos">Center tile of the coal power plant</param>
        private void coalSmoke(Position pos)
        {
            short[] SmTb = {
                (short) MapTileCharacters.COALSMOKE1, (short) MapTileCharacters.COALSMOKE2,
                (short) MapTileCharacters.COALSMOKE3, (short) MapTileCharacters.COALSMOKE4,
            };
            short[] dx = { 1, 2, 1, 2 };
            short[] dy = { -1, -1, 0, 0 };

            for (short x = 0; x < 4; x++)
            {
                //oldMap[pos.posX + dx[x],pos.posY + dy[x]] = (ushort)(SmTb[x] | (ushort)MapTileBits.CONDBIT |
                //    (ushort)MapTileBits.PWRBIT | (ushort)MapTileBits.BURNBIT);

                Vector3 position = new Vector3(pos.posX + dx[x], 0, pos.posY + dy[x]);
				map[position].Id = SmTb[x];
				map[position].IsPower = true;
				map[position].CanConduct = true;
				map[position].CanLit = true;
			}
		}

        /// <summary>
        /// Repair a zone at pos.
        /// </summary>
        /// <param name="pos">Center-tile position of the zone.</param>
        /// <param name="zCent">Value of the center tile.</param>
        /// <param name="zSize">Size of the zone (in both directions).</param>
        private void repairZone(Position pos, ushort zCent, short zSize)
        {
            ushort tile = (ushort) (zCent - 2 - zSize);

            // y and x loops one position shifted to compensate for the center-tile position.
            for (short y = -1; y < zSize - 1; y++)
            {
                for (short x = -1; x < zSize - 1; x++)
                {

                    int xx = pos.posX + x;
                    int yy = pos.posY + y;

                    tile++;

                    if (Position.testBounds((short) xx, (short) yy))
                    {
                        Vector3 position = new Vector3(xx, 0, yy);

                        //ushort mapValue = oldMap[xx,yy];
                        if (map.ContainsKey(position))
                        {
                            if (map[position].IsCenter == true)
                            {
                                continue;
                            }

                            //ushort mapTile = (ushort)(mapValue & (ushort) MapTileBits.LOMASK);

                            if (map[position].Id < (ushort)MapTileCharacters.RUBBLE || map[position].Id >= (ushort)MapTileCharacters.ROADBASE)
                            {
                                //oldMap[xx,yy] = (ushort) (tile | (ushort) MapTileBits.CONDBIT | (ushort) MapTileBits.BURNBIT);

                                map[position].Id = tile;
                                map[position].CanConduct = true;
                                map[position].CanLit = true;
                            }
                        }
					}
                }
            }
        }

        /// <summary>
        /// Handle repairing or removing of hospitals and churches.
        /// </summary>
        /// <param name="pos">Position of the hospital or church.</param>
        private void doHospitalChurch(Position pos)
        {
            //ushort tile = (ushort)(oldMap[pos.posX,pos.posY] & (ushort) MapTileBits.LOMASK);
			int tile = map[new Vector3(pos.posX, 0, pos.posY)].Id;

			if (tile == (ushort) MapTileCharacters.HOSPITAL)
            {

                hospitalPop++;

                if ((cityTime & 15) == 0)
                {
                    repairZone(pos, (ushort) MapTileCharacters.HOSPITAL, 3);
                }

                if (needHospital == -1)
                { 
                    // Too many hospitals!
                    if (getRandom(20) == 0)
                    {
                        zonePlop(pos, (short) MapTileCharacters.RESBASE); // Remove hospital.
                    }
                }

            }
            else if (tile == (ushort) MapTileCharacters.CHURCH)
            {

                churchPop++;

                //printf("CHURCH %d %d %d %d\n", churchPop, pos.posX, pos.posY, tile);

                if ((cityTime & 15) == 0)
                {
                    repairZone(pos, (ushort)tile, 3);
                }

                if (needChurch == -1)
                { 
                    // Too many churches!
                    if (getRandom(20) == 0)
                    {
                        zonePlop(pos, (short) MapTileCharacters.RESBASE); // Remove church.
                    }
                }
            }

        }

        /// <summary>
        /// Handle residential zone.
        /// </summary>
        /// <param name="pos">Center tile of the residential zone.</param>
        /// <param name="zonePower">Does the zone have power?</param>
        private void doResidential(Position pos, bool zonePower)
        {
            short tpop, zscore, locvalue, value, TrfGood;

            resZonePop++;

            Vector3 center = new Vector3(pos.posX, 0, pos.posY);
			TileInfo tile = map[center];

            City.InitZone(Zone.Residential, tile, center);

			if (tile != null)
			{
				Residential residential = tile.Tile.GetComponent<Residential>();
				zonePower = residential.Processor.IsElectro;
			}



			if (tile.Id == (ushort) MapTileCharacters.FREEZ)
            {
                tpop = doFreePop(pos);
            }
            else
            {
                tpop = getResZonePop((ushort)tile.Id);
            }

            resPop += tpop;

            if (tpop > getRandom(35))
            {
                // Try driving from residential to commercial
                TrfGood = makeTraffic(pos, ZoneType.ZT_COMMERCIAL);
            }
            else
            {
                TrfGood = 1;
            }

            if (TrfGood == -1)
            {
                value = getLandPollutionValue(pos);
                doResOut(pos, tpop, value);
                return;
            }

            if (tile.Id == (ushort) MapTileCharacters.FREEZ || (getRandom16() & 7) != 0)
            {
                locvalue = evalRes(pos, TrfGood);
                zscore = (short) (resValve + locvalue);

                if (!zonePower)
                {
                    zscore -= 500;
                }

                if (zscore > -350 &&
                    ((short) (zscore - 26380) > ((short) getRandom16Signed())))
                {
                    if (tpop != 0 && (getRandom16() & 3) != 0)
                    {
                        makeHospital(pos);
                        return;
                    }

                    value = getLandPollutionValue(pos);
                    doResIn(pos, tpop, value);
                    return;
                }

                if (zscore < 350 &&
                    (((short) (zscore + 26380)) < ((short) getRandom16Signed())))
                {
                    value = getLandPollutionValue(pos);
                    doResOut(pos, tpop, value);
                }
            }
        }

        /// <summary>
        /// Perform residential immigration into the current residential tile.
        /// </summary>
        /// <param name="pos">Position of the tile.</param>
        /// <param name="pop">Population ?</param>
        /// <param name="value">Land value corrected for pollution.</param>
        private void doResIn(Position pos, int pop, int value)
        {
            short population = populationDensityMap.worldGet(pos.posX, pos.posY);

            if (population > 128)
            {
                return;
            }

			int tile = map[new Vector3(pos.posX, 0, pos.posY)].Id;

			if (tile == (ushort) MapTileCharacters.FREEZ)
            {
                if (pop < 8)
                {
                    buildHouse(pos, value);
                    incRateOfGrowth(pos, 1);
                    return;
                }

                byte densityPop = populationDensityMap.worldGet(pos.posX, pos.posY);
				if (densityPop > 64)
                {
                    resPlop(pos, 0, value);
                    incRateOfGrowth(pos, 8);
                    return;
                }

                return;
            }

            if (pop < 40)
            {
                resPlop(pos, (pop / 8) - 1, value);
                incRateOfGrowth(pos, 8);
            }
        }

        /// <summary>
        /// Try to build a house at the zone at pos.
        /// TODO Have some form of looking around the center tile (like getFromMap())
        /// </summary>
        /// <param name="pos">Center tile of the zone.</param>
        /// <param name="value">Value to build (land value?)</param>
        private void buildHouse(Position pos, int value)
        {
            short z, score, hscore, BestLoc;
            short[] ZeX = {0, -1, 0, 1, -1, 1, -1, 0, 1};
            short[] ZeY = {0, -1, -1, -1, 0, 0, 1, 1, 1};

            BestLoc = 0;
            hscore = 0;

            for (z = 1; z < 9; z++)
            {
                int xx = pos.posX + ZeX[z];
                int yy = pos.posY + ZeY[z];

                if (Position.testBounds((short) xx, (short) yy))
                {

                    score = evalLot(xx, yy);

                    // BUG score is never 0 !!
                    if (score != 0)
                    {
                        if (score > hscore)
                        {
                            hscore = score;
                            BestLoc = z;
                        }

                        // TODO Move the code below to a better place.
                        //      If we just updated hscore above, we could
                        //      trigger this code too.
                        if (score == hscore && (getRandom16() & 7) == 0)
                        {
                            BestLoc = z;
                        }
                    }
                }
            }

            if (BestLoc != 0)
            {
                int xx = pos.posX + ZeX[BestLoc];
                int yy = pos.posY + ZeY[BestLoc];

                TileInfo tileInfo = map[new Vector3(pos.posX, 0, pos.posY)];
                if (tileInfo != null)
                {
                    Residential residential = tileInfo.Tile.GetComponent<Residential>();

                    int type = getRandom(2) + value * 3;
                    Vector3 offset = new Vector3(ZeX[BestLoc], 0, ZeY[BestLoc]) + new Vector3(-1, 0, -1);
					residential.AddSingleHouse(type, offset);
                }

                /*if (Position.testBounds((short) xx, (short) yy))
                {
					Vector3 position = new Vector3(xx, 0, yy);
                    if (map.ContainsKey(position))
                    {
                        map[position].Id = (ushort)((ushort)MapTileCharacters.HOUSE + getRandom(2) + value * 3);
                        map[position].CanConduct = true;
                        map[position].CanLit = true;
                        map[position].IsBulldozable = true;
                    }
				}*/
			}
        }

        /// <summary>
        /// Evaluate suitability of the position for placing a new house.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Suitability.</returns>
        private short evalLot(int x, int y)
        {
            short score = 1;
            short[] DX = {0, 1, 0, -1};
            short[] DY = {-1, 0, 1, 0};

			// test for clear lot
			//z = (short) (oldMap[x, y] & (ushort) MapTileBits.LOMASK);

			Vector3 position = new Vector3(x, 0, y);
            if (map.ContainsKey(position))
            {
                TileInfo tile0 = map[position];

                //int z = map[new Vector3(x, 0, y)].Id;

                if (tile0.Id > 0 && (tile0.Id < (ushort)MapTileCharacters.RESBASE || tile0.Id > (ushort)MapTileCharacters.RESBASE + 8))
                {
                    return -1;
                }

                for (int z = 0; z < 4; z++)
                {
                    int xx = x + DX[z];
                    int yy = y + DY[z];

                    if (Position.testBounds((short)xx, (short)yy))
                    {
                        TileInfo tile = map[new Vector3(xx, 0, yy)];

                        if (tile.Id != (ushort)MapTileCharacters.DIRT &&
                            tile.Id <= (ushort)MapTileCharacters.LASTROAD)
                        {
                            score++; // look for road
                        }
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// Put down a residential zone.
        /// </summary>
        /// <param name="pos">Center tile of the residential zone.</param>
        /// <param name="den">Population density (0..3)</param>
        /// <param name="value">Land value - pollution (0..3), higher is better.</param>
        private void resPlop(Position pos, int den, int value)
        {
            short baseValue;

            baseValue = (short) ((value * 4 + den) * 9 + MapTileCharacters.RZB - 4);
            zonePlop(pos, baseValue);
        }

        /// <summary>
        /// Put down a 3x3 zone around the center tile at pos..
        /// BUG This function allows partial on-map construction. Is that intentional? No!
        /// </summary>
        /// <param name="pos">Position to center around</param>
        /// <param name="baseValue">Tile number of the top-left tile.</param>
        /// <returns>Build was a success.</returns>
        private bool zonePlop(Position pos, short baseValue)
        {
            short z, x;
            short[] Zx = {-1, 0, 1, -1, 0, 1, -1, 0, 1};
            short[] Zy = {-1, -1, -1, 0, 0, 0, 1, 1, 1};

            for (z = 0; z < 9; z++)
            {
                // check for fire
                int xx = pos.posX + Zx[z];
                int yy = pos.posY + Zy[z];

                if (Position.testBounds((short) xx, (short) yy))
                {
                    Vector3 p = new Vector3(xx, 0, yy);
                    if (map.ContainsKey(p))
                    {
                        int t = map[p].Id;
                        if (t >= (ushort)MapTileCharacters.FLOOD && t < (ushort)MapTileCharacters.ROADBASE)
                        {
                            return false;
                        }
                    }
                }
            }

            /*
            for (z = 0; z < 9; z++)
            {
                int xx = pos.posX + Zx[z];
                int yy = pos.posY + Zy[z];

                if (Position.testBounds((short) xx, (short) yy))
                {
                    //oldMap[xx, yy] = (ushort)(baseValue + (ushort) MapTileBits.BNCNBIT);
                    map[new Vector3(xx, 0, yy)].Id = baseValue;
					map[new Vector3(xx, 0, yy)].CanConduct = true;
					map[new Vector3(xx, 0, yy)].CanLit = true;
				}

				baseValue++;
            }

            //setZonePower(pos);
            //oldMap[pos.posX, pos.posY] |= (ushort)MapTileBits.ZONEBIT + (ushort) MapTileBits.BULLBIT;
			map[new Vector3(pos.posX, 0, pos.posY)].IsCenter = true;
			map[new Vector3(pos.posX, 0, pos.posY)].IsBulldozable = true;
            */

            if (baseValue >= 612 && baseValue <= 684)
            { 
				TileInfo tileInfo = map[new Vector3(pos.posX, 0, pos.posY)];
				if (tileInfo != null)
				{
					int index = (baseValue - 612) / 9;
					Industrial industrial = tileInfo.Tile.GetComponent<Industrial>();
                    tileInfo.Id = baseValue + 4;
                    tileInfo.IsChanged = false;
					industrial.ChangeLevel(index);
				}
			}



			return true;
        }

        /// <summary>
        /// Update the rate of growth at position pos by amount.
        /// </summary>
        /// <param name="pos">Position to modify.</param>
        /// <param name="amount">Amount of change (can both be positive and negative).</param>
        private void incRateOfGrowth(Position pos, int amount)
        {
            int value = rateOfGrowthMap.worldGet(pos.posX, pos.posY);

            value = clamp(value + amount * 4, -200, 200);
            rateOfGrowthMap.worldSet(pos.posX, pos.posY, (short) value);
        }

        /// <summary>
        /// Evaluate residential zone.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="traf"></param>
        /// <returns></returns>
        private short evalRes(Position pos, int traf)
        {
            short value;

            if (traf < 0)
            {
                return -3000;
            }

            value = landValueMap.worldGet(pos.posX, pos.posY);
            value -= pollutionDensityMap.worldGet(pos.posX, pos.posY);

            if (value < 0)
            {
                value = 0; // Cap at 0
            }
            else
            {
                value = (short) Math.Min(value * 32, 6000); // Cap at 6000
            }

            value = (short) (value - 3000);

            return value;
        }

        /// <summary>
        /// If needed, add a new hospital or a new church.
        /// TODO this doubles for churches and might need to be fixed from the original C code
        /// </summary>
        /// <param name="pos">Center position of the new hospital or church.</param>
        private void makeHospital(Position pos)
        {
            if (needHospital > 0)
            {
                //Закомментированна сама постройка, т.к. не предсказуемо поведет себя, но не удалять
                //zonePlop(pos, (short) (MapTileCharacters.HOSPITAL - 4));
                needHospital = 0;
                return;
            }

            /*
             * add churches later
             * 
            if (needChurch > 0)
            {
                int churchType = getRandom(7); // 0 to 7 inclusive
                int tile;
                if (churchType == 0)
                {
                    tile = CHURCH0;
                }
                else
                {
                    tile = CHURCH1 + ((churchType - 1) * 9);
                }

                //printf("NEW CHURCH tile %d x %d y %d type %d\n", tile, pos.posX, pos.posY, churchType);

                zonePlop(pos, tile - 4);
                needChurch = 0;
                return;
            }
            */
        }

        /// <summary>
        /// Perform residential emigration from the current residential tile.
        /// </summary>
        /// <param name="pos">Position of the tile.</param>
        /// <param name="pop">Population ?</param>
        /// <param name="value">Land value corrected for pollution.</param>
        private void doResOut(Position pos, int pop, int value)
        {
            short[] Brdr = {0, 3, 6, 1, 4, 7, 2, 5, 8};
            short x, y, loc, z;

            if (pop == 0)
            {
                return;
            }

            if (pop > 16)
            {
                resPlop(pos, (pop - 24) / 8, value);
                incRateOfGrowth(pos, -8);
                return;
            }

            if (pop == 16)
            {
                incRateOfGrowth(pos, -8);
                //oldMap[pos.posX,pos.posY] = (ushort) MapTileCharacters.FREEZ | (ushort)MapTileBits.BLBNCNBIT | (ushort)MapTileBits.IsCenter;

                Vector3 position = new Vector3(pos.posX, 0, pos.posY);
                map[position].Id = (ushort)MapTileCharacters.FREEZ;
				map[position].IsCenter = true;
				map[position].IsBulldozable = true;
				map[position].CanConduct = true;
				map[position].CanLit = true;


				for (x = (short) (pos.posX - 1); x <= pos.posX + 1; x++)
                {
                    for (y = (short) (pos.posY - 1); y <= pos.posY + 1; y++)
                    {
                        if (Position.testBounds(x, y))
                        {
							Vector3 position2 = new Vector3(x, 0, y);

							/*if ((oldMap[x,y] & (ushort) MapTileBits.LOMASK) != (ushort) MapTileCharacters.FREEZ)
                            {
                                oldMap[x,y] = (ushort)(MapTileCharacters.LHTHR + value + getRandom(2) + (ushort) MapTileBits.BLBNCNBIT);
                            }*/
							if (map[position2].Id != (ushort)MapTileCharacters.FREEZ)
							{
								//oldMap[x, y] = (ushort)(MapTileCharacters.LHTHR + value + getRandom(2) + (ushort)MapTileBits.BLBNCNBIT);
								map[position2].Id = (ushort)(MapTileCharacters.LHTHR + value + getRandom(2));
								map[position2].IsBulldozable = true;
								map[position2].CanConduct = true;
								map[position2].CanLit = true;
							}
						}
					}
                }
            }

            if (pop < 16)
            {
                incRateOfGrowth(pos, -1);
                /*z = 0;
				for (x = (short) (pos.posX - 1); x <= pos.posX + 1; x++)
                {
                    for (y = (short) (pos.posY - 1); y <= pos.posY + 1; y++)
                    {
                        if (Position.testBounds(x, y))
                        {
                            //loc = (short)(oldMap[x,y] & (ushort) MapTileBits.LOMASK);
							loc = (short)map[new Vector3(x, 0, y)].Id;
							if ((loc >= (ushort) MapTileCharacters.LHTHR) && (loc <= (ushort) MapTileCharacters.HHTHR))
                            {
                                //oldMap[x,y] = (ushort)(Brdr[z] + (ushort) MapTileBits.BLBNCNBIT + (ushort) MapTileCharacters.FREEZ - 4);

								map[new Vector3(x, 0, y)].Id = (ushort)(Brdr[z] + (ushort)MapTileCharacters.FREEZ - 4);
								map[new Vector3(x, 0, y)].IsBulldozable = true;
								map[new Vector3(x, 0, y)].CanLit = true;
								map[new Vector3(x, 0, y)].CanConduct = true;

								return;
                            }
                        }
                        z++;
                    }
                }*/
				TileInfo tileInfo = map[new Vector3(pos.posX, 0, pos.posY)];
				if (tileInfo != null)
				{
					Residential residential = tileInfo.Tile.GetComponent<Residential>();
					residential.RemoveSingleHouse();
				}

			}
		}

        /// <summary>
        /// Compute land value at pos, taking pollution into account.
        /// </summary>
        /// <param name="pos">Position of interest.</param>
        /// <returns>Indication of land-value adjusted for pollution (0 >= low value, 3 => high value)</returns>
        private short getLandPollutionValue(Position pos)
        {
            short landVal;

            landVal = landValueMap.worldGet(pos.posX, pos.posY);
            landVal -= pollutionDensityMap.worldGet(pos.posX, pos.posY);

            if (landVal < 30)
            {
                return 0;
            }

            if (landVal < 80)
            {
                return 1;
            }

            if (landVal < 150)
            {
                return 2;
            }

            return 3;
        }

        /// <summary>
        /// Return population of a residential zone center tile
        /// (RZB, RZB+9, ..., HOSPITAL - 9).
        /// </summary>
        /// <param name="mapTile">Center tile of a residential zone.</param>
        /// <returns>Population of the residential zone. (16, 24, 32, 40, 16, ..., 40 )</returns>
        private short getResZonePop(ushort mapTile)
        {
            short CzDen = (short)((mapTile - (short) MapTileCharacters.RZB) / 9 % 4);

            return (short) (CzDen * 8 + 16);
        }

        /// <summary>
        /// Count the number of single tile houses in a residential zone.
        /// </summary>
        /// <param name="pos">Position of the residential zone.</param>
        /// <returns>Number of single tile houses.</returns>
        private short doFreePop(Position pos)
        {
            int count = 0;

            /*for (short x = (short) (pos.posX - 1); x <= pos.posX + 1; x++)
            {
                for (short y = (short) (pos.posY - 1); y <= pos.posY + 1; y++)
                {
                    if (x >= 0 && x < WORLD_W && y >= 0 && y < WORLD_H)
                    {
                        //ushort tile = (ushort)(oldMap[x,y] & (ushort) MapTileBits.LOMASK);
						//ushort tile = (ushort)map[new Vector3(x, 0, y)].Id;
						Vector3 position = new Vector3(x, 0, y);
                        if (map.ContainsKey(position))
                        {
                            TileInfo tile = map[position];

                            if (tile.Id >= (ushort)MapTileCharacters.LHTHR && tile.Id <= (ushort)MapTileCharacters.HHTHR)
                            {
                                count++;
                            }
                        }
                    }
                }
            }*/

            TileInfo tileInfo = map[new Vector3(pos.posX, 0, pos.posY)];
            if (tileInfo != null)
            {
                Residential residential = tileInfo.Tile.GetComponent<Residential>();
                count = residential.GetSingleHouseCount();
            }

			return (short)count;
        }

        /// <summary>
        /// Copy the value of powerGridMap at position pos to the map.
        /// </summary>
        /// <param name="pos">Position to copy.</param>
        /// <returns>Does the tile have power?</returns>
        /*private bool setZonePower(Position pos)
        {
           // ushort mapValue = oldMap[pos.posX, pos.posY];
            //ushort tile = (ushort)(mapValue & (ushort) MapTileBits.LOMASK);

			Vector3 position = new Vector3(pos.posX, 0, pos.posY);

			if (map[position].Id == (ushort) MapTileCharacters.NUCLEAR || map[position].Id == (ushort) MapTileCharacters.POWERPLANT)
            {
                //oldMap[pos.posX, pos.posY] = (ushort)(mapValue | (ushort) MapTileBits.PWRBIT);
                //map[position].Id = mapValue;
				map[position].IsPower = true;
				return true;
            }

            if (powerGridMap.worldGet(pos.posX, pos.posY) > 0)
            {
                //oldMap[pos.posX, pos.posY] = (ushort) (mapValue | (ushort) MapTileBits.PWRBIT);
				//map[position].Id = mapValue;
				map[position].IsPower = true;
				return true;
            }
            else
            {
                //oldMap[pos.posX, pos.posY] = (ushort)(mapValue & ~(ushort)MapTileBits.PWRBIT);
				//map[position].Id = mapValue;
				map[position].IsPower = false;
				return false;
            }
        }*/

        /// <summary>
        /// Get commercial zone population number.
        /// </summary>
        /// <param name="tile">Tile of the commercial zone.</param>
        /// <returns>Population number of the zone.</returns>
        short getComZonePop(ushort tile)
        {
            if (tile == (ushort) MapTileCharacters.COMCLR)
            {
                return 0;
            }

            short CzDen = (short) (((tile - (ushort) MapTileCharacters.CZB) / 9) % 5 + 1);
            return CzDen;
        }

        /// <summary>
        /// Get the population value for the given industrial tile.
        /// </summary>
        /// <param name="tile">Center tile value of the industrial zone.</param>
        /// <returns>Population value.</returns>
        short getIndZonePop(ushort tile)
        {
            if (tile == (ushort)MapTileCharacters.INDCLR)
            {
                return 0;
            }

            short CzDen = (short)(((tile - (ushort)MapTileCharacters.IZB) / 9) % 4 + 1);
            return CzDen;
        }
    }
}