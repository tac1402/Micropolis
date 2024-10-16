﻿namespace MicropolisCore
{
    /// <summary>
    /// Characters of the map tiles, the lower 10 bits (0--9).
    /// TODO Add TILE_ prefix
    /// TODO Make LOW/BASE and LAST/HIGH consistent everywhere?
    /// TODO Figure out what sprite groups really exist (maybe we can learn more by
    ///      examining the actual sprites, and/or by using hexadecimal or bite-wise
    ///      notation?)
    /// TODO Add comments for each sprite (0--1023)
    /// </summary>
    public enum MapTileCharacters
    {
        // Одиночные 0-238, 827 - 951
        
        // Рельеф
		DIRT = 0, // Clear tile
        // tile 1 ?

        /* Water */
        RIVER = 2,
        REDGE = 3,
        CHANNEL = 4,
        FIRSTRIVEDGE = 5,
        // tile 6 -- 19 ?
        LASTRIVEDGE = 20,
        WATER_LOW = RIVER,       // First water tile
        WATER_HIGH = LASTRIVEDGE, // Last water tile (inclusive)

        TREEBASE = 21,
        WOODS_LOW = TREEBASE,
        LASTTREE = 36,
        WOODS = 37,
        //UNUSED_TRASH1 = 38,
        UNUSED_TRASH2 = 39,
        WOODS_HIGH = UNUSED_TRASH2, // Why is an 'UNUSED' tile used?
        WOODS2 = 40,
        WOODS3 = 41,
        WOODS4 = 42,
        WOODS5 = 43,

        // Rubble Обломки 
        RUBBLE = 44,
        LASTRUBBLE = 47,

        // Наводнение 
        FLOOD = 48,
        // tile 49, 50 ?
        LASTFLOOD = 51,

        RADTILE = 52, // Radio-active contaminated tile

        UNUSED_TRASH3 = 53,
        UNUSED_TRASH4 = 54,
        UNUSED_TRASH5 = 55,

        /* Fire animation (8 tiles) */
        FIRE = 56,
        FIREBASE = FIRE,
        LASTFIRE = 63,

        // Дорога

        HBRIDGE = 64, // Horizontal bridge
        ROADBASE = HBRIDGE,
        VBRIDGE = 65, // Vertical bridge
        ROADS = 66,
        ROADS2 = 67,
        ROADS3 = 68,
        ROADS4 = 69,
        ROADS5 = 70,
        ROADS6 = 71,
        ROADS7 = 72,
        ROADS8 = 73,
        ROADS9 = 74,
        ROADS10 = 75,
        INTERSECTION = 76,
        HROADPOWER = 77,
        VROADPOWER = 78,
        BRWH = 79,
        LTRFBASE = 80, // First tile with low traffic
        // tile 81 -- 94 ?
        BRWV = 95,
        // tile 96 -- 110 ?
        BRWXXX1 = 111,
        // tile 96 -- 110 ?
        BRWXXX2 = 127,
        // tile 96 -- 110 ?
        BRWXXX3 = 143,
        HTRFBASE = 144, // First tile with high traffic
        // tile 145 -- 158 ?
        BRWXXX4 = 159,
        // tile 160 -- 174 ?
        BRWXXX5 = 175,
        // tile 176 -- 190 ?
        BRWXXX6 = 191,
        // tile 192 -- 205 ?
        LASTROAD = 206,
        BRWXXX7 = 207,

		/* Линии электропередач */
		HPOWER = 208,
        //VPOWER = 209,
        //LHPOWER = 210,
        //LVPOWER = 211,
        //LVPOWER2 = 212,
        //LVPOWER3 = 213,
        //LVPOWER4 = 214,
        //LVPOWER5 = 215,
        //LVPOWER6 = 216,
        //LVPOWER7 = 217,
        //LVPOWER8 = 218,
        //LVPOWER9 = 219,
        LVPOWER10 = 220,
        //RAILHPOWERV = 221, // Horizontal rail, vertical power
        RAILVPOWERH = 222, // Vertical rail, horizontal power
        POWERBASE = HPOWER,
        LASTPOWER = RAILVPOWERH,

        UNUSED_TRASH6 = 223,

        /* Rail (железная дорога) */
        HRAIL = 224,
        VRAIL = 225,
        LHRAIL = 226,
        LVRAIL = 227,
        LVRAIL2 = 228,
        LVRAIL3 = 229,
        LVRAIL4 = 230,
        LVRAIL5 = 231,
        LVRAIL6 = 232,
        LVRAIL7 = 233,
        LVRAIL8 = 234,
        LVRAIL9 = 235,
        LVRAIL10 = 236,
        HRAILROAD = 237,
        VRAILROAD = 238,
        RAILBASE = HRAIL,
        LASTRAIL = 238,

        //ROADVPOWERH = 239, /* bogus? */

        // Residential zone tiles

        RESBASE = 240, // Empty residential, tiles 240--248
        FREEZ = 244, // center-tile of 3x3 empty residential

        HOUSE = 249, // Single tile houses until 260
        LHTHR = HOUSE,
        HHTHR = 260,

        RZB = 265, // center tile first 3x3 tile residential 
		// R1 - 261-269 - 16
		// R2 - 270-278 - 24
		// R3 - 279-287 - 32
		// R4 - 288-296 - 40
		// R5 - 298-305 - 16
		// R6 - 307-314 - 24
		// R7 - 316-323 - 32
		// R8 - 324-332 - 40
		// R9  - 333-341 - 16
		// R10 - 342-350 - 24
		// R11 - 351-359 - 32
		// R12 - 360-368 - 40
		// R13 - 369-377 - 16
		// R14 - 378-386 - 24
		// R15 - 387-395 - 32
		// R16 - 396-404 - 40

		HOSPITALBASE = 405, // Center of hospital (tiles 405--413)
        HOSPITAL = 409, // Center of hospital (tiles 405--413)

        //CHURCHBASE = 414, // Center of church (tiles 414--422)
        //CHURCH0BASE = 414, // numbered alias
        CHURCH = 418, // Center of church (tiles 414--422)
        //CHURCH0 = 418, // numbered alias

        // Commercial zone tiles

        COMBASE = 423, // Empty commercial, tiles 423--431
                       // tile 424 -- 426 ?
        COMCLR = 427,
		// tile 428 -- 435 ?

		// C0  - 423 - 431 - 0
		// C1  - 432 - 440 - 1
		// C2  - 441 - 449 - 2
		// C3  - 450 - 458 - 3
		// C4  - 459 - 467 - 4
		// C5  - 468 - 476 - 5
		// C6  - 477 - 485 - 1
		// C7  - 486 - 494 - 2
		// C8  - 495 - 503 - 3
		// C9  - 504 - 512 - 4
		// C10 - 513 - 521 - 5
		// C11 - 522 - 530 - 1
		// C12 - 531 - 539 - 2
		// C13 - 540 - 548 - 3
		// C14 - 549 - 557 - 4
		// C15 - 558 - 566 - 5
		// C16 - 567 - 575 - 1
		// C17 - 576 - 584 - 2
		// C18 - 585 - 593 - 3 
		// C19 - 594 - 602 - 4
		// C20 - 603 - 611 - 5


		CZB = 436,
        // tile 437 -- 608 ?


        // COMLAST = 609,
        // tile 610, 611 ?

        // Industrial zone tiles.
        INDBASE = 612, // Top-left tile of empty industrial zone.
        INDCLR = 616, // Center tile of empty industrial zone.
        LASTIND = 620, // Last tile of empty industrial zone.

		// I - 612 - 620
		// I1 - 621 - 629 - 1
		// I2 - 630 - 638 - 2
		// I3 - 639 - 647 - 3
		// I4 - 648 - 656 - 4
		// I5 - 657 - 665 - 1
		// I6 - 666 - 674 - 2
		// I7 - 675 - 683 - 3
		// I8 - 684 - 692 - 4

		// Industrial zone population 0, value 0: 621 -- 629
		IND1 = 621, // Top-left tile of first non-empty industry zone.
        IZB = 625, // Center tile of first non-empty industry zone.

        // Industrial zone population 1, value 0: 630 -- 638

        // Industrial zone population 2, value 0: 639 -- 647
        IND2 = 641,
        IND3 = 644,

        // Industrial zone population 3, value 0: 648 -- 656
        IND4 = 649,
        IND5 = 650,

        // Industrial zone population 0, value 1: 657 -- 665

        // Industrial zone population 1, value 1: 666 -- 674

        // Industrial zone population 2, value 1: 675 -- 683
        IND6 = 676,
        IND7 = 677,

        // Industrial zone population 3, value 1: 684 -- 692
        IND8 = 686,
        IND9 = 689,

        // Seaport (4x4) 693 - 708
        PORTBASE = 693, // Top-left tile of the seaport.
        PORT = 698, // Center tile of the seaport.
        //LASTPORT = 708, // Last tile of the seaport.

        // Airport (6x6) 709 - 744
        //AIRPORTBASE = 709,
        //RADAR = 711,
        AIRPORT = 716,

        // Coal power plant (4x4) 745-760
        //COALBASE = 745, // First tile of coal power plant.
        POWERPLANT = 750, // 'Center' tile of coal power plant.
        LASTPOWERPLANT = 760, // Last tile of coal power plant.

        // Fire station (3x3) 761-769
        //FIRESTBASE = 761, // First tile of fire station.
        FIRESTATION = 765, // 'Center tile' of fire station.
						   // 769 last tile fire station.

		// Police station (3x3) 770-778
		POLICESTBASE = 770,
        POLICESTATION = 774,

        // StadiumEmpty (4x4) 779 - 794
        //STADIUMBASE = 779, // First tile stadium.
        STADIUM = 784, // 'Center tile' stadium.

		// StadiumFull (4x4) 795 - 810
		FULLSTADIUM = 800,

        // Nuclear power plant (4x4) 811 - 826
        //NUCLEARBASE = 811, // First tile nuclear power plant.
        NUCLEAR = 816, // 'Center' tile nuclear power plant.
        LASTZONE = 826, // Also last tile nuclear power plant.

        // Знак "нет электричества"
        LIGHTNINGBOLT = 827,

        HBRDG0 = 828,
        HBRDG1 = 829,
        HBRDG2 = 830,
        HBRDG3 = 831,
        HBRDG_END = 832,
		//RADAR0 = 832,
		//RADAR1 = 833,
		//RADAR2 = 834,
		//RADAR3 = 835,
		//RADAR4 = 836,
		//RADAR5 = 837,
		//RADAR6 = 838,
		//RADAR7 = 839,
		//FOUNTAIN = 840,
		// tile 841 -- 843: fountain animation.
		//INDBASE2 = 844,
		//TELEBASE = 844,
		// tile 845 -- 850 ?
		//TELELAST = 851,
		SMOKEBEGIN = 852,
		SMOKEBASE = 852,
        // tile 853 -- 859 ?
        //TINYEXP = 860,
        // tile 861 -- 863 ?
        SOMETINYEXP = 864,
        // tile 865 -- 866 ?
        LASTTINYEXP = 867,
        // tile 868 -- 882 ?
        //TINYEXPLAST = 883,
        // tile 884 -- 915 ?

        COALSMOKE1 = 916, // Chimney animation at coal power plant (2, 0).
                          // 919 last animation tile for chimney at coal power plant (2, 0).

        COALSMOKE2 = 920, // Chimney animation at coal power plant (3, 0).
                          // 923 last animation tile for chimney at coal power plant (3, 0).

        COALSMOKE3 = 924, // Chimney animation at coal power plant (2, 1).
                          // 927 last animation tile for chimney at coal power plant (2, 1).

        COALSMOKE4 = 928, // Chimney animation at coal power plant (3, 1).
                          // 931 last animation tile for chimney at coal power plant (3, 1).

        //FOOTBALLGAME1 = 932,
        // tile 933 -- 939 ?
        //FOOTBALLGAME2 = 940,
        // tile 941 -- 947 ?
        VBRDG0 = 948,
        VBRDG1 = 949,
        VBRDG2 = 950,
        VBRDG3 = 951,

        //NUKESWIRL1 = 952,
        //NUKESWIRL2 = 953,
        //NUKESWIRL3 = 954,
        //NUKESWIRL4 = 955,

        // Tiles 956-959 unused (originally)
        TILE_COUNT     = 960,

        // Extended zones: 956-1019

//        CHURCH1BASE = 956,
//        CHURCH1 = 960,
//        CHURCH2BASE = 965,
//        CHURCH2 = 969,
//        CHURCH3BASE = 974,
//        CHURCH3 = 978,
//        CHURCH4BASE = 983,
//        CHURCH4 = 987,
//        CHURCH5BASE = 992,
//        CHURCH5 = 996,
//        CHURCH6BASE = 1001,
//        CHURCH6 = 1005,
//        CHURCH7BASE = 1010,
//        CHURCH7 = 1014,
//        CHURCH7LAST = 1018,

        // Tiles 1020-1023 unused

//        TILE_COUNT = 1024,

        TILE_INVALID = -1, // Invalid tile (not used in the world map).
    }
}