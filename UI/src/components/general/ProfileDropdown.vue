<template>
    <div class="flex-container">
        <div class="icon-container">
            <h2 class="container">Username placeholder</h2>
            <img :src="pictureSource" alt="" class="profile-image">
        </div>
        <router-link to="/dashboard/profile" class="icon-container container">
            <h2 class="institution-name container">Edit profile </h2>
            <v-icon name="md-arrowforwardios-round" scale="1.5" :fill="iconFill"></v-icon>
        </router-link>
        <div class="button-container container">
            <button class="log-out">Log out
                <v-icon name="ri-logout-box-r-line" scale="1.5"></v-icon>
            </button>
        </div>
    </div>
</template>
<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { fetchProfilePicture } from '../../api/apiService';
import {iconFill} from '../../GlobalVariables'

const style = getComputedStyle(document.body)
const pictureSource = ref<string>()

onMounted(async () => {
    pictureSource.value = await fetchProfilePicture()

    if(pictureSource.value === ''){
        pictureSource.value = style.getPropertyValue('--user-icon')
        console.log(pictureSource.value)
    }
    console.log(pictureSource.value)
})
</script>
<style scoped>
.flex-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: space-evenly;
    color: var(--font-color-primary);
    background-color: var(--secondary-color);
    font-family: var(--main-font);
    padding: 1rem;
    gap: 1rem;
    width: 20rem;
    height: 17rem;
    border-radius: 5px;
}

.container,
.icon-container {
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    max-height: 3rem;
    border-radius: 5px;
}

.container {
    background-color: var(--primary-color);
    padding: 0.5rem;

}

.icon-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    /* max-height: 4rem; */

}

.icon-container .container {
    width: 80%;
}

.log-out {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.log-out:hover {
    color: var(--decline-button);
}

.profile-image {
    width: 3rem;
    height: 3rem;
}
</style>