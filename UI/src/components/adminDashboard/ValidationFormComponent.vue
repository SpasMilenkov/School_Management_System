<script setup lang="ts">
import { ref, onMounted, inject } from 'vue'
import axios from 'axios'
import { Emitter } from 'mitt';

type RenewEvent = {
  adding?:User,
  renewUser?:boolean,
}

const isTeacher = ref(false)
const emitter:Emitter<RenewEvent> = inject('emitter')
const user = ref<User>({
id: 0,
name: '',
email: '',
role: '',
})
const userRole = ref('student')
const selectedFaculty = ref('')
const selectedGroup = ref('')
const selectedSpecialty = ref('')


const props = defineProps(['faculties', 'groups', 'specialties'])
function onSubmit(){}
const title = ref('')
onMounted(() => {
  emitter.on('adding', (userInfo:User) => {
    if (userInfo.role === 'student') {
      isTeacher.value = false
      userRole.value = 'student'
    } else {
      isTeacher.value = true
      userRole.value = 'teacher'
    }

    user.value = userInfo
  })
})
async function addUser() {
  if (isTeacher.value) {
    try {
      console.log(title.value)
      const result = await axios({
        method: 'POST',
        data: { title: title.value, userId: user.value.id },
        url: `https://localhost:7080/api/Admin/approve/teacher`,
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      })
      console.log(result.data)
      return result.data
    } catch (e) {
      console.log(e)
    }
  } else {
    try {
      const result = await axios({
        method: 'POST',
        data: {
          userId: user.value.id,
          email: user.value.email,
          specialty: selectedSpecialty.value,
          faculty: selectedFaculty.value,
          group: selectedGroup.value,
          course: 1
        },
        url: `https://localhost:7080/api/Admin/approve/student`,
        headers: {
          'Content-Type': 'application/json'
        },
        withCredentials: true
      })
      console.log(result.data)
      return result.data
    } catch (e) {
      console.log(e)
    }
  }
  emitter.emit('renewUser', true)
}
</script>
<template>
  <div class="form-container shadow">
    <form action="" id="student-form" class="form" v-on:submit.prevent="onSubmit">
      <h2 class="subtitle">Add {{ userRole }}:</h2>
      <div class="input-wrapper">
        <h3 class="subtitle">Name:</h3>
        <input type="text" name="name" id="name-input" class="input-field" :value="user.name" />
      </div>
      <div class="input-wrapper">
        <h3 class="subtitle">Email:</h3>
        <input type="text" name="email" id="email-input" class="input-field" :value="user.email" />
      </div>
      <div class="input-wrapper">
        <h3 class="subtitle">Id:</h3>
        <h3 class="subtitle input-field">{{ user.id }}</h3>
      </div>
      <div class="input-wrapper" v-if="isTeacher">
        <label for="title-input">Title:</label>
        <input type="text" name="title" id="title-input" class="input-field" v-model="title" />
      </div>
      <div class="input-wrapper" v-if="!isTeacher">
        <label for="faculty-input">Faculty</label>
        <div class="select-wrapper">
          <v-select
            class="style-chooser"
            :options="faculties"
            label="name"
            :reduce="(faculty) => faculty.id"
            id="faculty-input"
            v-model="selectedFaculty"
          ></v-select>
        </div>
      </div>
      <div class="input-wrapper" v-if="!isTeacher">
        <label for="group-input">Group</label>
        <div class="select-wrapper">
          <v-select
            class="style-chooser"
            :options="groups"
            id="group-input"
            v-model="selectedGroup"
          ></v-select>
        </div>
      </div>
      <div class="input-wrapper" v-if="!isTeacher">
        <label for="specialty-input">Specialty</label>
        <div class="select-wrapper">
          <v-select
            class="style-chooser"
            :options="specialties"
            label="name"
            :reduce="(specialty) => specialty.id"
            v-model="selectedSpecialty"
          ></v-select>
        </div>
      </div>
      <button class="submit-button add-button" @click="addUser">Approve</button>
    </form>
  </div>
</template>
<style scoped>
.input-field {
  min-width: 15.3rem;
}
 .form-container {
  max-height: none;
  max-width: none;
}
</style>
