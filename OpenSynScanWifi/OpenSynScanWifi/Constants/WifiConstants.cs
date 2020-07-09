using OpenSynScanWifi.Annotations;

namespace OpenSynScanWifi.Constants
{
	public static class WifiConstants
	{
		/*
		 * The server only ever sends messages that start with
		 *
		 * (ASCII)		[HEX]			<DEC>
		 *   (=)		[3d]			<61>
		 *   (!)		[21]			<33>			This seems to only ever be (!0\r) [21 30 0d] <33 48 13>
		 *   (+)		[2b]			<43>            Could be that this is a response plus a status?
		 */
		/*
		 * The client sends messages that start with
		 *
		 * (ASCII)		[HEX]			<DEC>
		 *   (:)		[3a]			<58>                All commands during the handshake use this
		 *   (AT+)		[41 54 2b]		<65 84 43>			Seems to query mode and status
		 */

		/// <summary>
		/// 3a 65 31 0d				:e1.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_DISCOVERY_DATAGRAM = new byte[] {58, 101, 49, 13};

		/// <summary>
		/// 3d 30 32 30 34 38		=020482.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_DISCOVERY_DATAGRAM = new byte[] {61, 48, 50, 48, 52, 56, 50, 13};

		/// <summary>
		/// Client opens a new port for this
		/// 3a 71 31 30 31 30		:q1010000.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_DISCOVERY_DATAGRAM2 = new byte[] {58, 113, 49, 48, 49, 48, 48, 48, 48, 13};

		/// <summary>
		/// 21 30 0d				!0.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_DISCOVERY_DATAGRAM2 = new byte[] {33, 48, 13};

		/// <summary>
		/// 3a 65 31 0d				:e2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_DISCOVERY_DATAGRAM3 = new byte[] {58, 101, 50, 13};

		/// <summary>
		/// 3d 30 32 30 34 38		=020482.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_DISCOVERY_DATAGRAM3 = new byte[] {61, 48, 50, 48, 52, 56, 50, 13};

		/// <summary>
		/// 3a 50 31 32 0d			:P12.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_DISCOVERY_DATAGRAM4 = new byte[] {58, 80, 49, 50, 13};

		/// <summary>
		/// 21 30 0d				!0.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_DISCOVERY_DATAGRAM4 = new byte[] {33, 48, 13};

		/// <summary>
		/// A Start character. 
		/// The SkyWatcher motor controller always resets its RX buffer whenever a start character is received.
		/// 
		/// 3a						:
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_START_DATAGRAM = new byte[] {58};

		/// <summary>
		/// 3a 50 32 32 0d			:P22.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_DISCOVERY_DATAGRAM5 = new byte[] {58, 80, 50, 50, 13};

		/// <summary>
		/// 21 30 0d				!0.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_DISCOVERY_DATAGRAM5 = new byte[] {33, 48, 13};

		/// <summary>
		/// 3a 61 31 0d				:a1.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_A1 = new byte[] {58, 97, 49, 13};

		/// <summary>
		/// 3d 31 38 35 33 32 30 0d	=185320.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_A1 = new byte[] {0x3d, 0x31, 0x38, 0x35, 0x33, 0x32, 0x30, 0x0d};

		/// <summary>
		/// 3a 61 32 0d				:a2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_A2 = new byte[] {0x3a, 0x61, 0x32, 0x0d};

		/// <summary>
		/// 3d 31 38 35 33 32 30 0d	=185320.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_A2 = new byte[] {0x3d, 0x31, 0x38, 0x35, 0x33, 0x32, 0x30, 0x0d};

		/// <summary>
		/// 3a 62 32 0d				:b2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_B1 = new byte[] {0x3a, 0x62, 0x32, 0x0d};

		/// <summary>
		/// 3d 34 42 34 43 30 30 0d	=4B4C00.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_B1 = new byte[] {0x3d, 0x34, 0x42, 0x34, 0x43, 0x30, 0x30, 0x0d};

		/// <summary>
		/// 3a 67 31 0d				:g1.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_G1 = new byte[] {0x3a, 0x67, 0x31, 0x0d};

		/// <summary>
		/// 3d 32 30 0d				=20.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_G1 = new byte[] {0x3d, 0x32, 0x30, 0x0d};

		/// <summary>
		/// 3a 67 32 0d				:g2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_G2 = new byte[] {0x3a, 0x67, 0x32, 0x0d};

		/// <summary>
		/// 3d 32 30 0d				=20.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_G2 = new byte[] {0x3d, 0x32, 0x30, 0x0d};

		/// <summary>
		/// 3a 46 31 0d				:F1.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_F1 = new byte[] {0x3a, 0x46, 0x31, 0x0d};

		/// <summary>
		/// 3d 0d				=.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_F1 = new byte[] {0x3d, 0x0d};

		/// <summary>
		/// 3a 46 32 0d				:F2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_F2 = new byte[] {0x3a, 0x46, 0x32, 0x0d};

		/// <summary>
		/// 3d 0d				=.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_F2 = new byte[] {0x3d, 0x0d};

		/// <summary>
		/// 3a 66 31 0d				:f1.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_f1 = new byte[] {0x3a, 0x66, 0x31, 0x0d};

		/// <summary>
		/// 3d 30 30 33 0d			=003.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_f1 = new byte[] {0x3d, 0x30, 0x30, 0x33, 0x0d};

		/// <summary>
		/// 3a 66 32 0d				:f2.
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_UNK_f2 = new byte[] {0x3a, 0x66, 0x31, 0x0d};

		/// <summary>
		/// 3d 30 30 33 0d			=003.
		/// </summary>
		[NotNull] public static readonly byte[] SMSG_UNK_f2 = new byte[] {0x3d, 0x30, 0x30, 0x33, 0x0d};

		/// <summary>
		/// 41 54 2b 43 57 4d 4f 44 45 5f 43 55 52 3f 0d 0a		AT+CWMODE_CUR?..
		/// </summary>
		[NotNull] public static readonly byte[] CMSG_MOUNT_MODE_QUERY = new byte[] {0x41, 0x54, 0x2b, 0x43, 0x57, 0x4d, 0x4f, 0x44, 0x45, 0x5f, 0x43, 0x55, 0x52, 0x3f, 0x0d, 0x0a};

		/// <summary>
		/// 2b 43 57 4d 4f 44 45 5f 43 55 52 3a 32 0d 0a 0d 0a 4f 4b 0d 0a	+CWMODE_CUR:2....OK..
		[NotNull] public static readonly byte[] SMSG_MOUNT_MODE_QUERY = new byte[] {0x2b, 0x43, 0x57, 0x4d, 0x4f, 0x44, 0x45, 0x5f, 0x43, 0x55, 0x52, 0x3a, 0x32, 0x0d, 0x0a, 0x0d, 0x0a, 0x4f, 0x4b, 0x0d, 0x0a};
	}
}