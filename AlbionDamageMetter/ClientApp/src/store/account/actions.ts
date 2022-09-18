import { ActionTree } from 'vuex'
import { AccountState } from './types'
import { RootState } from '../types'

export const actions: ActionTree<AccountState, RootState> = {
  setAuthenticated({ commit }, isAuthenticated: boolean) {
    commit('setAuthenticated', isAuthenticated)
  },
  setDiscordId({ commit }, discordId: string) {
    commit('setDiscordId', discordId)
  },
  setDiscordAvatarUrl({ commit }, discordAvatarUrl: string) {
    commit('setDiscordAvatarUrl', discordAvatarUrl)
  },
  reset({ commit }) {
    commit('reset')
  }
}
