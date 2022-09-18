import { Module } from 'vuex'
import { getters } from './getters'
import { actions } from './actions'
import { mutations } from './mutations'
import { AccountState } from './types'
import { RootState } from '../types'

export const state: AccountState = {
  isAuthenticated: false,
  discordId: '',
  discordAvatarUrl: ''
}

const namespaced = true

export const account: Module<AccountState, RootState> = {
  namespaced,
  state,
  getters,
  actions,
  mutations
}
