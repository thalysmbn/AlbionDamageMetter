import { MutationTree } from 'vuex'
import { AccountState } from './types'

export const mutations: MutationTree<AccountState> = {
  setAuthenticated(state, isAuthenticated: boolean) {
    state.isAuthenticated = isAuthenticated
  },
  setDiscordId(state, discordId: string) {
    state.discordId = discordId
  },
  setDiscordAvatarUrl(state, discordAvatarUrl: string) {
    state.discordAvatarUrl = discordAvatarUrl
  }
}
