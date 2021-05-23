/*
TShock, a server mod for Terraria
Copyright (C) 2011-2019 Pryaxis & TShock Contributors

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;
using Terraria;
using TShockAPI;
using Terraria.Localization;
using Terraria.GameContent.NetModules;
using Terraria.Net;
using Terraria.ID;

namespace TShockAPI
{
	public class PlayerData
	{
		public NetItem[] inventory = new NetItem[NetItem.MaxInventory];
		public int health = TShock.ServerSideCharacterConfig.Settings.StartingHealth;
		public int maxHealth = TShock.ServerSideCharacterConfig.Settings.StartingHealth;
		public int mana = TShock.ServerSideCharacterConfig.Settings.StartingMana;
		public int maxMana = TShock.ServerSideCharacterConfig.Settings.StartingMana;
		public bool exists;
		public int spawnX = -1;
		public int spawnY = -1;
		public int? extraSlot;
		public int? skinVariant;
		public int? hair;
		public byte hairDye;
		public Color? hairColor;
		public Color? pantsColor;
		public Color? shirtColor;
		public Color? underShirtColor;
		public Color? shoeColor;
		public Color? skinColor;
		public Color? eyeColor;
		public bool[] hideVisuals;
		public int questsCompleted;
		public int usingBiomeTorches;
		public int happyFunTorchTime = 1;
		public int unlockedBiomeTorches;

		public PlayerData(TSPlayer player)
		{
			for (int i = 0; i < NetItem.MaxInventory; i++)
			{
				this.inventory[i] = new NetItem();
			}

			for (int i = 0; i < TShock.ServerSideCharacterConfig.Settings.StartingInventory.Count; i++)
			{
				var item = TShock.ServerSideCharacterConfig.Settings.StartingInventory[i];
				StoreSlot(i, item.NetId, item.PrefixId, item.Stack);
			}
		}

		/// <summary>
		/// Stores an item at the specific storage slot
		/// </summary>
		/// <param name="slot"></param>
		/// <param name="netID"></param>
		/// <param name="prefix"></param>
		/// <param name="stack"></param>
		public void StoreSlot(int slot, int netID, byte prefix, int stack)
		{
			if (slot > (this.inventory.Length - 1)) //if the slot is out of range then dont save
			{
				return;
			}

			this.inventory[slot] = new NetItem(netID, stack, prefix);
		}

		/// <summary>
		/// Copies a characters data to this object
		/// </summary>
		/// <param name="player"></param>
		public void CopyCharacter(TSPlayer player)
		{
			this.health = player.TPlayer.statLife > 0 ? player.TPlayer.statLife : 1;
			this.maxHealth = player.TPlayer.statLifeMax;
			this.mana = player.TPlayer.statMana;
			this.maxMana = player.TPlayer.statManaMax;
			if (player.sX > 0 && player.sY > 0)
			{
				this.spawnX = player.sX;
				this.spawnY = player.sY;
			}
			else
			{
				this.spawnX = player.TPlayer.SpawnX;
				this.spawnY = player.TPlayer.SpawnY;
			}
			extraSlot = player.TPlayer.extraAccessory ? 1 : 0;
			this.skinVariant = player.TPlayer.skinVariant;
			this.hair = player.TPlayer.hair;
			this.hairDye = player.TPlayer.hairDye;
			this.hairColor = player.TPlayer.hairColor;
			this.pantsColor = player.TPlayer.pantsColor;
			this.shirtColor = player.TPlayer.shirtColor;
			this.underShirtColor = player.TPlayer.underShirtColor;
			this.shoeColor = player.TPlayer.shoeColor;
			this.hideVisuals = player.TPlayer.hideVisibleAccessory;
			this.skinColor = player.TPlayer.skinColor;
			this.eyeColor = player.TPlayer.eyeColor;
			this.questsCompleted = player.TPlayer.anglerQuestsFinished;
			this.usingBiomeTorches = player.TPlayer.UsingBiomeTorches ? 1 : 0;
			this.happyFunTorchTime = player.TPlayer.happyFunTorchTime ? 1 : 0;
			this.unlockedBiomeTorches = player.TPlayer.unlockedBiomeTorches ? 1 : 0;

			Item[] inventory = player.TPlayer.inventory;
			Item[] armor = player.TPlayer.armor;
			Item[] dye = player.TPlayer.dye;
			Item[] miscEqups = player.TPlayer.miscEquips;
			Item[] miscDyes = player.TPlayer.miscDyes;
			Item[] piggy = player.TPlayer.bank.item;
			Item[] safe = player.TPlayer.bank2.item;
			Item[] forge = player.TPlayer.bank3.item;
			Item[] voidVault = player.TPlayer.bank4.item;
			Item trash = player.TPlayer.trashItem;

			for (int i = 0; i < NetItem.MaxInventory; i++)
			{
				if (i < NetItem.InventoryIndex.Item2)
				{
					//0-58
					this.inventory[i] = (NetItem)inventory[i];
				}
				else if (i < NetItem.ArmorIndex.Item2)
				{
					//59-78
					var index = i - NetItem.ArmorIndex.Item1;
					this.inventory[i] = (NetItem)armor[index];
				}
				else if (i < NetItem.DyeIndex.Item2)
				{
					//79-88
					var index = i - NetItem.DyeIndex.Item1;
					this.inventory[i] = (NetItem)dye[index];
				}
				else if (i < NetItem.MiscEquipIndex.Item2)
				{
					//89-93
					var index = i - NetItem.MiscEquipIndex.Item1;
					this.inventory[i] = (NetItem)miscEqups[index];
				}
				else if (i < NetItem.MiscDyeIndex.Item2)
				{
					//93-98
					var index = i - NetItem.MiscDyeIndex.Item1;
					this.inventory[i] = (NetItem)miscDyes[index];
				}
				else if (i < NetItem.PiggyIndex.Item2)
				{
					//98-138
					var index = i - NetItem.PiggyIndex.Item1;
					this.inventory[i] = (NetItem)piggy[index];
				}
				else if (i < NetItem.SafeIndex.Item2)
				{
					//138-178
					var index = i - NetItem.SafeIndex.Item1;
					this.inventory[i] = (NetItem)safe[index];
				}
				else if (i < NetItem.TrashIndex.Item2)
				{
					//179-219
					this.inventory[i] = (NetItem)trash;
				}
				else if (i < NetItem.ForgeIndex.Item2)
				{
					//220
					var index = i - NetItem.ForgeIndex.Item1;
					this.inventory[i] = (NetItem)forge[index];
				}
				else
				{
					//220
					var index = i - NetItem.VoidIndex.Item1;
					this.inventory[i] = (NetItem)voidVault[index];
				}
			}
		}

		/// <summary>
		/// Restores a player's character to the state stored in the database
		/// </summary>
		/// <param name="player"></param>
		public void RestoreCharacter(TSPlayer player)
		{
			// Start ignoring SSC-related packets! This is critical so that we don't send or receive dirty data!
			player.IgnoreSSCPackets = true;

			player.TPlayer.statLife = this.health;
			player.TPlayer.statLifeMax = this.maxHealth;
			player.TPlayer.statMana = this.maxMana;
			player.TPlayer.statManaMax = this.maxMana;
			player.TPlayer.SpawnX = this.spawnX;
			player.TPlayer.SpawnY = this.spawnY;
			player.sX = this.spawnX;
			player.sY = this.spawnY;
			player.TPlayer.hairDye = this.hairDye;
			player.TPlayer.anglerQuestsFinished = this.questsCompleted;
			player.TPlayer.UsingBiomeTorches = this.usingBiomeTorches == 1;
			player.TPlayer.happyFunTorchTime = this.happyFunTorchTime == 1;
			player.TPlayer.unlockedBiomeTorches = this.unlockedBiomeTorches == 1;

			if (extraSlot != null)
				player.TPlayer.extraAccessory = extraSlot.Value == 1 ? true : false;
			if (this.skinVariant != null)
				player.TPlayer.skinVariant = this.skinVariant.Value;
			if (this.hair != null)
				player.TPlayer.hair = this.hair.Value;
			if (this.hairColor != null)
				player.TPlayer.hairColor = this.hairColor.Value;
			if (this.pantsColor != null)
				player.TPlayer.pantsColor = this.pantsColor.Value;
			if (this.shirtColor != null)
				player.TPlayer.shirtColor = this.shirtColor.Value;
			if (this.underShirtColor != null)
				player.TPlayer.underShirtColor = this.underShirtColor.Value;
			if (this.shoeColor != null)
				player.TPlayer.shoeColor = this.shoeColor.Value;
			if (this.skinColor != null)
				player.TPlayer.skinColor = this.skinColor.Value;
			if (this.eyeColor != null)
				player.TPlayer.eyeColor = this.eyeColor.Value;

			if (this.hideVisuals != null)
				player.TPlayer.hideVisibleAccessory = this.hideVisuals;
			else
				player.TPlayer.hideVisibleAccessory = new bool[player.TPlayer.hideVisibleAccessory.Length];

			for (int i = 0; i < NetItem.MaxInventory; i++)
			{
				if (i < NetItem.InventoryIndex.Item2)
				{
					//0-58
					player.TPlayer.inventory[i].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.inventory[i].netID != 0)
					{
						player.TPlayer.inventory[i].stack = this.inventory[i].Stack;
						player.TPlayer.inventory[i].prefix = this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.ArmorIndex.Item2)
				{
					//59-78
					var index = i - NetItem.ArmorIndex.Item1;
					player.TPlayer.armor[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.armor[index].netID != 0)
					{
						player.TPlayer.armor[index].stack = this.inventory[i].Stack;
						player.TPlayer.armor[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.DyeIndex.Item2)
				{
					//79-88
					var index = i - NetItem.DyeIndex.Item1;
					player.TPlayer.dye[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.dye[index].netID != 0)
					{
						player.TPlayer.dye[index].stack = this.inventory[i].Stack;
						player.TPlayer.dye[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.MiscEquipIndex.Item2)
				{
					//89-93
					var index = i - NetItem.MiscEquipIndex.Item1;
					player.TPlayer.miscEquips[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.miscEquips[index].netID != 0)
					{
						player.TPlayer.miscEquips[index].stack = this.inventory[i].Stack;
						player.TPlayer.miscEquips[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.MiscDyeIndex.Item2)
				{
					//93-98
					var index = i - NetItem.MiscDyeIndex.Item1;
					player.TPlayer.miscDyes[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.miscDyes[index].netID != 0)
					{
						player.TPlayer.miscDyes[index].stack = this.inventory[i].Stack;
						player.TPlayer.miscDyes[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.PiggyIndex.Item2)
				{
					//98-138
					var index = i - NetItem.PiggyIndex.Item1;
					player.TPlayer.bank.item[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.bank.item[index].netID != 0)
					{
						player.TPlayer.bank.item[index].stack = this.inventory[i].Stack;
						player.TPlayer.bank.item[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.SafeIndex.Item2)
				{
					//138-178
					var index = i - NetItem.SafeIndex.Item1;
					player.TPlayer.bank2.item[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.bank2.item[index].netID != 0)
					{
						player.TPlayer.bank2.item[index].stack = this.inventory[i].Stack;
						player.TPlayer.bank2.item[index].prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.TrashIndex.Item2)
				{
					//179-219
					var index = i - NetItem.TrashIndex.Item1;
					player.TPlayer.trashItem.netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.trashItem.netID != 0)
					{
						player.TPlayer.trashItem.stack = this.inventory[i].Stack;
						player.TPlayer.trashItem.prefix = (byte)this.inventory[i].PrefixId;
					}
				}
				else if (i < NetItem.ForgeIndex.Item2)
				{
					//220
					var index = i - NetItem.ForgeIndex.Item1;
					player.TPlayer.bank3.item[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.bank3.item[index].netID != 0)
					{
						player.TPlayer.bank3.item[index].stack = this.inventory[i].Stack;
						player.TPlayer.bank3.item[index].Prefix((byte)this.inventory[i].PrefixId);
					}
				}
				else
				{
					//260
					var index = i - NetItem.VoidIndex.Item1;
					player.TPlayer.bank4.item[index].netDefaults(this.inventory[i].NetId);

					if (player.TPlayer.bank4.item[index].netID != 0)
					{
						player.TPlayer.bank4.item[index].stack = this.inventory[i].Stack;
						player.TPlayer.bank4.item[index].Prefix((byte)this.inventory[i].PrefixId);
					}
				}
			}

			float slot = 0f;
			for (int k = 0; k < NetItem.InventorySlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].inventory[k].Name), player.Index, slot, (float)Main.player[player.Index].inventory[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.ArmorSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].armor[k].Name), player.Index, slot, (float)Main.player[player.Index].armor[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.DyeSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].dye[k].Name), player.Index, slot, (float)Main.player[player.Index].dye[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.MiscEquipSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].miscEquips[k].Name), player.Index, slot, (float)Main.player[player.Index].miscEquips[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.MiscDyeSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].miscDyes[k].Name), player.Index, slot, (float)Main.player[player.Index].miscDyes[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.PiggySlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].bank.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank.item[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.SafeSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].bank2.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank2.item[k].prefix);
				slot++;
			}
			NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].trashItem.Name), player.Index, slot++, (float)Main.player[player.Index].trashItem.prefix);
			for (int k = 0; k < NetItem.ForgeSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].bank3.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank3.item[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.VoidSlots; k++)
			{
				NetMessage.SendData(5, -1, -1, NetworkText.FromLiteral(Main.player[player.Index].bank4.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank4.item[k].prefix);
				slot++;
			}


			NetMessage.SendData(4, -1, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
			NetMessage.SendData(42, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
			NetMessage.SendData(16, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);

			slot = 0f;
			for (int k = 0; k < NetItem.InventorySlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].inventory[k].Name), player.Index, slot, (float)Main.player[player.Index].inventory[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.ArmorSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].armor[k].Name), player.Index, slot, (float)Main.player[player.Index].armor[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.DyeSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].dye[k].Name), player.Index, slot, (float)Main.player[player.Index].dye[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.MiscEquipSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].miscEquips[k].Name), player.Index, slot, (float)Main.player[player.Index].miscEquips[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.MiscDyeSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].miscDyes[k].Name), player.Index, slot, (float)Main.player[player.Index].miscDyes[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.PiggySlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].bank.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank.item[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.SafeSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].bank2.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank2.item[k].prefix);
				slot++;
			}
			NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].trashItem.Name), player.Index, slot++, (float)Main.player[player.Index].trashItem.prefix);
			for (int k = 0; k < NetItem.ForgeSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].bank3.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank3.item[k].prefix);
				slot++;
			}
			for (int k = 0; k < NetItem.VoidSlots; k++)
			{
				NetMessage.SendData(5, player.Index, -1, NetworkText.FromLiteral(Main.player[player.Index].bank4.item[k].Name), player.Index, slot, (float)Main.player[player.Index].bank4.item[k].prefix);
				slot++;
			}



			NetMessage.SendData(4, player.Index, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
			NetMessage.SendData(42, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
			NetMessage.SendData(16, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);

			for (int k = 0; k < 22; k++)
			{
				player.TPlayer.buffType[k] = 0;
			}

			/*
			 * The following packets are sent twice because the server will not send a packet to a client
			 * if they have not spawned yet if the remoteclient is -1
			 * This is for when players login via uuid or serverpassword instead of via
			 * the login command.
			 */
			NetMessage.SendData(50, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
			NetMessage.SendData(50, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);

			NetMessage.SendData(76, player.Index, -1, NetworkText.Empty, player.Index);
			NetMessage.SendData(76, -1, -1, NetworkText.Empty, player.Index);

			NetMessage.SendData(39, player.Index, -1, NetworkText.Empty, 400);

			if (Main.GameModeInfo.IsJourneyMode)
			{
				var sacrificedItems = TShock.ResearchDatastore.GetSacrificedItems();
				for(int i = 0; i < ItemID.Count; i++)
				{
					var amount = 0;
					if (sacrificedItems.ContainsKey(i))
					{
						amount = sacrificedItems[i];
					}

					var response = NetCreativeUnlocksModule.SerializeItemSacrifice(i, amount);
					NetManager.Instance.SendToClient(response, player.Index);
				}
			}
		}
	}
}
