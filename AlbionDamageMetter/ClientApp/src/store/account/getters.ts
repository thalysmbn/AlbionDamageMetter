import { GetterTree } from 'vuex'
import { AccountState } from './types'
import { RootState } from '../types'

export const getters: GetterTree<AccountState, RootState> = {
  isAuthenticated(state): boolean {
    return state.isAuthenticated
  },
  discordId(state): string {
    return state.discordId
  },
  discordAvatarUrl(state): string {
    return state.discordAvatarUrl
  }
}
