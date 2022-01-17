from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
import pandas as pd
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC


class Solution:
   def solve(self, s):
      seen = s[0]
      ans = s[0]
      for i in s[1:]:
         if i != seen:
            ans += i
            seen = i
      return ans



if __name__ == '__main__':
    """
    option = webdriver.ChromeOptions()
    option.binary_location = r'C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe'
    driver = webdriver.Chrome(executable_path=r'./chromedriver', options=option)
    driver.get("https://www.google.com")
    """
    driver = webdriver.Chrome('./chromedriver')
    owl_type = "NamedIndividual "
    driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
    oa = driver.find_elements(By.XPATH, '//div[@id="affix-osnovne"]/div[@class="panel-body"]/a')
    os = driver.find_elements(By.XPATH, '//div[@id="affix-specijalisticke"]/div[@class="panel-body"]/a')
    ma = driver.find_elements(By.XPATH, '//div[@id="affix-master"]/div[@class="panel-body"]/a')
    ms = driver.find_elements(By.XPATH, '//div[@id="affix-master-strukovne"]/div[@class="panel-body"]/a')
    doc = driver.find_elements(By.XPATH, '//div[@id="affix-doktorske"]/div[@class="panel-body"]/a')

    rdf_about_url = "rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"

    courses = []
    subjects = []
    type_of_resource_aiiso_schema_c = "aiiso/schema#Programme"
    type_of_resource_aiiso_schema_p = "aiiso/schema#Course"
    type_of_resource_aiiso_schema_t = "aiiso/schema#Teaches"
    type_of_resource_aiiso_schema_f = "aiiso/schema#Faculty"
    type_of_resource_aiiso_schema_a = "aiiso/schema#Assistent"

    for c in range(len(oa)):
        driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
        oa1 = driver.find_elements(By.XPATH, '//div[@id="affix-osnovne"]/div[@class="panel-body"]/a')
        oa = oa1
        active_course = oa[c].text.replace(" ","_") 
        courses.append("<owl:" + owl_type + " " +rdf_about_url + active_course + "\">\n")

        driver.execute_script("window.open('"+oa[c].get_attribute("href")+"')")
        driver.switch_to.window(driver.window_handles[-1])
        subList = driver.find_elements(By.XPATH, '//a[starts-with(@id,\'viewPredmet\')]')

        panel = driver.find_elements(By.ID, 'affix-info')
        elements_from_panel = panel[0].text.split('\n')
	
        #courses.append("\t<untitled-ontology-7:Title rdf:datatype=\"&xsd;string\">" + elements_from_panel[4] + "</untitled-ontology-7:Title>\n")
        #courses.append("\t<untitled-ontology-7:Educational_Field rdf:datatype=\"&xsd;string\">" + elements_from_panel[6] + "</untitled-ontology-7:Educational_Field>\n")
        #courses.append("\t<untitled-ontology-7:Scientific_And_Professional_Fields rdf:datatype=\"&xsd;string\">" + elements_from_panel[8] + "</untitled-ontology-7:Scientific_And_Professional_Fields>\n")
        #courses.append("\t<untitled-ontology-7:Year_Semester rdf:datatype=\"&xsd;int\">" + elements_from_panel[10] + "</untitled-ontology-7:Year_Semester>\n")
        #courses.append("\t<untitled-ontology-7:Total_ESP_Points rdf:datatype=\"&xsd;int\">" + elements_from_panel[12] + "</untitled-ontology-7:Total_ESP_Points>\n")
        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_c + "\"/>\n")
        courses.append("</owl:"+owl_type+">\n\n")

        courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_course+"\"><Level_Of_Study rdf:datatype=\"&xsd;string\">" + elements_from_panel[2] + " </Level_Of_Study></rdf:Description>")

        esp_points_pc = driver.find_elements_by_css_selector("[title^='ESPB']")
        fond_predavanja = driver.find_elements_by_css_selector("[title^='Fond predavanja']")
        fond_auditaninih = driver.find_elements_by_css_selector("[title^='Fond auditornih vežbi']")
        count = 0
        for sub in subList:
            subjects.append("\n<owl:NamedIndividual " + rdf_about_url + sub.text.replace(" ","_") + "\">\n")
            
            subjects.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_p +"\"/>\n")
            subjects.append("</owl:NamedIndividual>\n\n")


            courses.append("\n<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+sub.text.replace(" ","_")+"\">\n<ESP_PointsPerCourse rdf:datatype=\"&xsd;integer\">" + esp_points_pc[count].text + " </ESP_PointsPerCourse>\n</rdf:Description>\n")
            courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+sub.text.replace(" ","_")+"\">\n<Found_Exercisess rdf:datatype=\"&xsd;string\">" + fond_auditaninih[count].text + " </Found_Exercisess>\n</rdf:Description>\n")
            courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+sub.text.replace(" ","_")+"\">\n<Found_Lectures rdf:datatype=\"&xsd;integer\">" + fond_predavanja[count].text + " </Found_Lectures>\n</rdf:Description>\n")


            count += 1
            subjects.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/Faculty_of_Technical_Sciences\"/>\n")
            subjects.append("<rdf:Description>\n")
            subjects.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
            subjects.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#contains\"/>\n")
            subjects.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + sub.text.replace(" ","_") + "\"/>\n")
            subjects.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/Faculty_of_Technical_Sciences\"/>\n")
            subjects.append("</rdf:Description>\n")

            subjects.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + sub.text.replace(" ","_") +"\"/>\n")
            subjects.append("<rdf:Description>\n")
            subjects.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
            subjects.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#courseBelonge\"/>\n")
            subjects.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_course + "\"/>\n")
            subjects.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + sub.text.replace(" ","_") +"\"/>\n")
            subjects.append("</rdf:Description>\n")
        
            # Inverse

            subjects.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + sub.text.replace(" ","_") + "\"/>\n")
            subjects.append("<rdf:Description>\n")
            subjects.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
            subjects.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#facultyContains\"/>\n")
            subjects.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/Faculty_of_Technical_Sciences\"/>\n")
            subjects.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + sub.text.replace(" ","_") + "\"/>\n")
            subjects.append("</rdf:Description>\n")

            subjects.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + active_course +"\"/>\n")
            subjects.append("<rdf:Description>\n")
            subjects.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
            subjects.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#programmeHas\"/>\n")
            subjects.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+ sub.text.replace(" ","_") + "\"/>\n")
            subjects.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + active_course +"\"/>\n")
            subjects.append("</rdf:Description>\n")
       
        a = 1
        while(True):
            try:     
                button = driver.find_element(By.ID, 'a' + str(a))
                button.click()
                tabel = driver.find_element(By.ID, 'modalDialogBody')
                links_to_teachers = tabel.find_elements(By.TAG_NAME, "a") 


                for i in range(len(links_to_teachers)): 
                    #items = WebDriverWait(driver, 20).until(EC.visibility_of_all_elements_located(links_to_teachers[i]))
                    #items.click()
                    #print(i)
                    
                    ob = Solution()
                    active_teacher = ob.solve(links_to_teachers[i].get_attribute('textContent')).replace("\t","").replace(" ","_").replace("\n","_")
                    #print(active_teacher)
                    #print(subList[a].text)
                    if active_teacher == "" or active_teacher == 'd' or active_teacher == 'D':
                        courses.append("<owl:" + owl_type + " " +rdf_about_url + active_teacher + "\">\n")
                        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_t +"\"/>\n")
                        courses.append("</owl:"+owl_type+">\n\n")
                        
                        if active_teacher != "":
                                courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_teacher+"\"/>\n")
                                courses.append("<rdf:Description>\n")
                                courses.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
                                courses.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#teachesAtCourse\"/>\n")
                                courses.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+subList[a].text.replace(" ","_") + "\"/>\n")
                                courses.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_teacher+"\"/>\n")
                                courses.append("</rdf:Description>\n")

                                courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + subList[a].text.replace(" ","_") +"\"/>\n")
                                courses.append("<rdf:Description>\n")
                                courses.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
                                courses.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#teachesHoldsCourse\"/>\n")
                                courses.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + active_teacher + "\"/>\n")
                                courses.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + subList[a].text.replace(" ","_") + "\"/>\n")
                                courses.append("</rdf:Description>\n")
                        else:
                            print("PRAZENO")
                    else: 
                        courses.append("<owl:" + owl_type + " " +rdf_about_url + active_teacher + "\">\n")
                        courses.append("\t<rdf:type rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Assistent\"/>\n")
                        courses.append("</owl:"+owl_type+">\n\n")
                        
                        if active_teacher != "":
                                courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_teacher+"\"/>\n")
                                courses.append("<rdf:Description>\n")
                                courses.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
                                courses.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#teachesAtCourse\"/>\n")
                                courses.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+subList[a].text.replace(" ","_") + "\"/>\n")
                                courses.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"+active_teacher+"\"/>\n")
                                courses.append("</rdf:Description>\n")

                                courses.append("<rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + subList[a].text.replace(" ","_") +"\"/>\n")
                                courses.append("<rdf:Description>\n")
                                courses.append("    <rdf:type rdf:resource=\"&owl;NegativePropertyAssertion\"/>\n")
                                courses.append("    <owl:assertionProperty rdf:resource=\"http://www.owl-ontologies.com/travel.owl#teachesHoldsCourse\"/>\n")
                                courses.append("    <owl:targetIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + active_teacher + "\"/>\n")
                                courses.append("    <owl:sourceIndividual rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/" + subList[a].text.replace(" ","_") + "\"/>\n")
                                courses.append("</rdf:Description>\n")
                        else:
                            print("PRAZENO")
                    

                a += 1
                
                dialog = driver.find_element(By.ID, "modalDialog")
                exit_button = dialog.find_elements_by_tag_name("button")
                WebDriverWait(driver, 30).until(EC.element_to_be_clickable((By.ID, "modalDialog")))
                exit_button[0].click()
                
            except Exception as e:
                print(e)
                break
        
        driver.execute_script("window.close()")
        driver.switch_to.window(driver.window_handles[0])

        
    for c in range(len(os)):
        driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
        os1 = driver.find_elements(By.XPATH, '//div[@id="affix-specijalisticke"]/div[@class="panel-body"]/a')
        os = os1

        courses.append(
            "<owl:" + owl_type + rdf_about_url + os[c].text.replace(" ","_") + "\">\n")
        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_c + "\"/>\n")
        courses.append("</owl:"+owl_type+">\n\n")

        driver.execute_script("window.open('"+os[c].get_attribute("href")+"')")
        driver.switch_to.window(driver.window_handles[-1])
        subList = driver.find_elements(By.XPATH, '//a[starts-with(@id,\'viewPredmet\')]')

        for sub in subList:
            subjects.append(
                "<owl:NamedIndividual rdf:about=\"" + rdf_about_url + sub.text.replace(" ","_") + "\">\n")
            subjects.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/aiiso/" + type_of_resource_aiiso_schema_p +"\"/>\n")
            subjects.append("</owl:NamedIndividual>\n\n")
        
        a = 1
        while(True):
            try:
                button = driver.find_element(By.ID, 'a' + str(a))
                button.click()
                tabel = driver.find_element(By.ID, 'modalDialogBody')
                links_to_teachers = tabel.find_elements_by_tag_name("a")

                for i in range(len(links_to_teachers)):                    
                    ob = Solution()
                    courses.append("<owl:" + owl_type + " " +rdf_about_url + ob.solve(links_to_teachers[i].get_attribute('textContent')).replace("\t","").replace(" ","_").replace("\n","_") + "\">\n")
                    courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_t + "\"/>\n")
                    courses.append("</owl:"+owl_type+">\n\n")
                a += 1
                
                dialog = driver.find_element(By.ID, "modalDialog")
                exit_button = dialog.find_elements_by_tag_name("button")
                WebDriverWait(driver, 30).until(EC.element_to_be_clickable((By.ID, "modalDialog")))
                exit_button[0].click()
                
            except Exception as e:
                break

        driver.execute_script("window.close()")
        driver.switch_to.window(driver.window_handles[0])
    for c in range(len(ma)):
        driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
        ma1 = driver.find_elements(By.XPATH, '//div[@id="affix-master"]/div[@class="panel-body"]/a')
        ma = ma1

        courses.append(
            "<owl:" + owl_type + rdf_about_url + ma[c].text.replace(" ","_") + "\">\n")
        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_c + "\"/>\n")
        courses.append("</owl:"+owl_type+">\n\n")

        driver.execute_script("window.open('"+ma[c].get_attribute("href")+"')")
        driver.switch_to.window(driver.window_handles[-1])
        subList = driver.find_elements(By.XPATH, '//a[starts-with(@id,\'viewPredmet\')]')

        for sub in subList:
            subjects.append(
                "<owl:NamedIndividual rdf:about=\"" + rdf_about_url + sub.text.replace(" ","_") + "\">\n")
            subjects.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/aiiso/" + type_of_resource_aiiso_schema_p +"\"/>\n")
            subjects.append("</owl:NamedIndividual>\n\n")
        
        a = 1
        while(True):
            try:
                button = driver.find_element(By.ID, 'a' + str(a))
                button.click()
                tabel = driver.find_element(By.ID, 'modalDialogBody')
                links_to_teachers = tabel.find_elements_by_tag_name("a")

                for i in range(len(links_to_teachers)):                    
                    ob = Solution()
                    courses.append("<owl:" + owl_type + " " +rdf_about_url + ob.solve(links_to_teachers[i].get_attribute('textContent')).replace("\t","").replace(" ","_").replace("\n","_") + "\">\n")
                    courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_t + "\"/>\n")
                    courses.append("</owl:"+owl_type+">\n\n")
                a += 1
                
                dialog = driver.find_element(By.ID, "modalDialog")
                exit_button = dialog.find_elements_by_tag_name("button")
                WebDriverWait(driver, 30).until(EC.element_to_be_clickable((By.ID, "modalDialog")))
                exit_button[0].click()
                
            except Exception as e:
                break

        driver.execute_script("window.close()")
        driver.switch_to.window(driver.window_handles[0])
        
    for c in range(len(ms)):
        driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
        ms1 = driver.find_elements(By.XPATH, '//div[@id="affix-master-strukovne"]/div[@class="panel-body"]/a')
        ms = ms1

        courses.append(
            "<owl:" + owl_type + rdf_about_url + ms[c].text.replace(" ","_") + "\">\n")
        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_c + "\"/>\n")
        courses.append("</owl:"+owl_type+">\n\n")

        driver.execute_script("window.open('"+ms[c].get_attribute("href")+"')")
        driver.switch_to.window(driver.window_handles[-1])
        subList = driver.find_elements(By.XPATH, '//a[starts-with(@id,\'viewPredmet\')]')

        for sub in subList:
            subjects.append(
                "<owl:NamedIndividual rdf:about=\"" + rdf_about_url +sub.text.replace(" ","_") + "\">\n")
            subjects.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_p +"\"/>\n")
            subjects.append("</owl:NamedIndividual>\n\n")

        a = 1
        while(True):
            try:
                button = driver.find_element(By.ID, 'a' + str(a))
                button.click()
                tabel = driver.find_element(By.ID, 'modalDialogBody')
                links_to_teachers = tabel.find_elements_by_tag_name("a")

                for i in range(len(links_to_teachers)):                    
                    ob = Solution()
                    courses.append("<owl:" + owl_type + " " +rdf_about_url + ob.solve(links_to_teachers[i].get_attribute('textContent')).replace("\t","").replace(" ","_").replace("\n","_") + "\">\n")
                    courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_t + "\"/>\n")
                    courses.append("</owl:"+owl_type+">\n\n")
                a += 1
                
                dialog = driver.find_element(By.ID, "modalDialog")
                exit_button = dialog.find_elements_by_tag_name("button")
                WebDriverWait(driver, 30).until(EC.element_to_be_clickable((By.ID, "modalDialog")))
                exit_button[0].click()
                
            except Exception as e:
                break

        driver.execute_script("window.close()")
        driver.switch_to.window(driver.window_handles[0])
    for c in range(len(doc)):
        driver.get("http://www.ftn.uns.ac.rs/1802705466/studijski-programi--akreditacija-2020-")
        doc1 = driver.find_elements(By.XPATH, '//div[@id="affix-doktorske"]/div[@class="panel-body"]/a')
        doc = doc1

        courses.append("<owl:" + owl_type + rdf_about_url + doc[c].text.replace(" ","_") + "\">\n")
        courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_c +"\"/>\n")
        courses.append("</owl:"+owl_type+">\n\n")

        driver.execute_script("window.open('"+doc[c].get_attribute("href")+"')")
        driver.switch_to.window(driver.window_handles[-1])
        subList = driver.find_elements(By.XPATH, '//a[starts-with(@id,\'viewPredmet\')]')

        for sub in subList:
            subjects.append(
                "<owl:NamedIndividual rdf:about=\"" + rdf_about_url + sub.text.replace(" ","_") + "\">\n")
            subjects.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_p +"\"/>\n")
            subjects.append("</owl:NamedIndividual>\n\n")
        
        a = 1
        while(True):
            try:
                button = driver.find_element(By.ID, 'a' + str(a))
                button.click()
                tabel = driver.find_element(By.ID, 'modalDialogBody')
                links_to_teachers = tabel.find_elements_by_tag_name("a")

                for i in range(len(links_to_teachers)):                    
                    ob = Solution()
                    courses.append("<owl:" + owl_type + " " +rdf_about_url + ob.solve(links_to_teachers[i].get_attribute('textContent')).replace("\t","").replace(" ","_").replace("\n","_") + "\">\n")
                    courses.append("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_t + "\"/>\n")
                    courses.append("</owl:"+owl_type+">\n\n")
                a += 1
                
                dialog = driver.find_element(By.ID, "modalDialog")
                exit_button = dialog.find_elements_by_tag_name("button")
                WebDriverWait(driver, 30).until(EC.element_to_be_clickable((By.ID, "modalDialog")))
                exit_button[0].click()
                
            except Exception as e:
                break

        driver.execute_script("window.close()")
        driver.switch_to.window(driver.window_handles[0])
    """
    file = open("owl_import_v6.owl", "w", encoding="utf-8")
    

    file.write("<!DOCTYPE rdf:RDF [\n <!ENTITY owl \"http://www.w3.org/2002/07/owl#\" >\n <!ENTITY xsd \"http://www.w3.org/2001/XMLSchema#\" >\n <!ENTITY rdfs \"http://www.w3.org/2000/01/rdf-schema#\" >\n <!ENTITY rdf \"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" >\n <!ENTITY untitled-ontology-7 \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#\" >\n ]>")
    file.write("\n<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns:rdfs=\"http://www.w3.org/2000/01/rdf-schema#\" xmlns:owl=\"http://www.w3.org/2002/07/owl#\" xmlns:daml=\"http://www.daml.org/2001/03/daml+oil#\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns=\"http://www.owl-ontologies.com/travel.owl#\" xml:base=\"http://www.owl-ontologies.com/travel.owl\">\n<owl:Ontology rdf:about=\"\">\n<owl:versionInfo rdf:datatype=\"http://www.w3.org/2001/XMLSchema#string\">1.0 by Holger Knublauch (holger@smi.stanford.edu)</owl:versionInfo>\n<rdfs:comment rdf:datatype=\"http://www.w3.org/2001/XMLSchema#string\">An example ontology for tutorial purposes.</rdfs:comment>\n</owl:Ontology>\n")
    
    
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;ESP_Points\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Level_Of_Study\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Title\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Educational_Field\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Scientific_And_Professional_Fields\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Year_Semester\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Total_ESP_Points\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;ESP_PointsPerCourse\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Found_Exercisess\"/> \n")
    file.write("<owl:DatatypeProperty rdf:about=\"&untitled-ontology-7;Found_Lectures\"/> \n")

    """
    file.write("\n<owl:ObjectProperty rdf:ID=\"teachesAtCourse\"\n>")
    file.write("\t<rdfs:comment>Teaches at specific course</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Teaches\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("</owl:ObjectProperty>\n")
    """

    file.write("\n<owl:ObjectProperty rdf:ID=\"teachesHoldsCourse\"\n>")
    file.write("\t<rdfs:comment>Teaches at specific course</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Teaches\" />\n")
    file.write("\t<rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\" />\n")
    file.write("\t<owl:inverseOf rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#teachesAtCourse\"/>")
    file.write("</owl:ObjectProperty>\n")

    file.write("""  <rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#teachesAtCourse\">\n
                        <rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\"/>\n
                    </rdf:Description>\n""")

    """
    file.write("\n<owl:ObjectProperty rdf:ID=\"courseBelonge\">\n")
    file.write("\t<rdfs:comment>Programme has Couse</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Programme\" />\n")
    file.write("</owl:ObjectProperty>\n\n")
    """

    file.write("\n<owl:ObjectProperty rdf:ID=\"programmeHas\">\n")
    file.write("\t<rdfs:comment>Programme has Couse</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Programme\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("\t<rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\" />\n")
    file.write("\t<owl:inverseOf rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#courseBelonge\"/>")
    file.write("</owl:ObjectProperty>\n\n")


    file.write("""  <rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#courseBelonge\">\n
                        <rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\"/>\n
                    </rdf:Description>\n""")
    """
    file.write("\n<owl:ObjectProperty rdf:ID=\"contains\">\n")
    file.write("\t<rdfs:comment>Faculty constains Course</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Faculty\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("</owl:ObjectProperty>\n\n")
    """

    file.write("\n<owl:ObjectProperty rdf:ID=\"facultyContains\">\n")
    file.write("\t<rdfs:comment>Faculty constains Course</rdfs:comment>\n")
    file.write("\t<rdfs:domain rdf:resource=\"http://purl.org/vocab/aiiso/schema#Course\" />\n")
    file.write("\t<rdfs:range  rdf:resource=\"http://purl.org/vocab/aiiso/schema#Faculty\" />\n")
    file.write("\t<rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\" />\n")
    file.write("\t<owl:inverseOf rdf:resource=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#contains\"/>")
    file.write("</owl:ObjectProperty>\n\n")

    file.write("""  <rdf:Description rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#contains\">\n
                        <rdfs:subPropertyOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#part_of\"/>\n
                    </rdf:Description>\n""")

    file.write("")
    
    file.write("\n<owl:Class rdf:about=\"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Assistent\">")
    file.write("\t<rdfs:subClassOf rdf:resource=\"http://purl.org/vocab/aiiso/schema#Teaches\"/>")
    file.write("\n</owl:Class>")

    file.write("<owl:NamedIndividual " + rdf_about_url +  "Faculty_of_Technical_Sciences\">\n")
    file.write("\t<rdf:type rdf:resource=\"http://purl.org/vocab/" + type_of_resource_aiiso_schema_f +"\"/>\n")
    file.write("</owl:NamedIndividual>\n\n")


    file.write("""     <owl:ObjectProperty rdf:about="&untitled-ontology-7;problemHasQuestion">
        <rdfs:subPropertyOf rdf:resource="http://purl.org/vocab/aiiso/schema#part_of"/>
    </owl:ObjectProperty>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#questionBelongToProblem -->

    <owl:ObjectProperty rdf:about="&untitled-ontology-7;questionBelongToProblem">
        <rdfs:subPropertyOf rdf:resource="http://purl.org/vocab/aiiso/schema#part_of"/>
        <owl:inverseOf rdf:resource="&untitled-ontology-7;ProblemHasQuestion"/>
    </owl:ObjectProperty>
    


    <!-- 
    ///////////////////////////////////////////////////////////////////////////////////////
    //
    // Data properties
    //
    ///////////////////////////////////////////////////////////////////////////////////////
     -->

    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#startTimeOfTest -->

    <owl:DatatypeProperty rdf:about="&untitled-ontology-7;startTimeOfTest"/>
    


    <!-- 
    ///////////////////////////////////////////////////////////////////////////////////////
    //
    // Classes
    //
    ///////////////////////////////////////////////////////////////////////////////////////
     -->

    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Answare -->

    <owl:Class rdf:about="&untitled-ontology-7;Answare"/>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#AnswareStudent -->

    <owl:Class rdf:about="&untitled-ontology-7;AnswareStudent"/>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Domain -->

    <owl:Class rdf:about="&untitled-ontology-7;Domain"/>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Problem -->

    <owl:Class rdf:about="&untitled-ontology-7;Problem"/>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Question -->

    <owl:Class rdf:about="&untitled-ontology-7;Question"/>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Test -->

    <owl:Class rdf:about="&untitled-ontology-7;Test"/>
    


    <!-- 
    ///////////////////////////////////////////////////////////////////////////////////////
    //
    // Individuals
    //
    ///////////////////////////////////////////////////////////////////////////////////////
     -->

    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Css -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;Css">
        <rdf:type rdf:resource="&untitled-ontology-7;Problem"/>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#DataBase -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;DataBase">
        <rdf:type rdf:resource="&untitled-ontology-7;Domain"/>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#HTML -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;HTML">
        <rdf:type rdf:resource="&untitled-ontology-7;Problem"/>
    </owl:NamedIndividual>
    <rdf:Description>
        <rdf:type rdf:resource="&owl;NegativePropertyAssertion"/>
        <owl:targetIndividual rdf:resource="&untitled-ontology-7;Css"/>
        <owl:sourceIndividual rdf:resource="&untitled-ontology-7;HTML"/>
        <owl:assertionProperty rdf:resource="&untitled-ontology-7;questionBelongToProblem"/>
    </rdf:Description>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#JavaScript -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;JavaScript">
        <rdf:type rdf:resource="&untitled-ontology-7;Problem"/>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Osnovne_akademske_studije -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;Osnovne_akademske_studije">
        <rdf:type rdf:resource="http://purl.org/vocab/aiiso/schema#Programme"/>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Web -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;Web">
        <rdf:type rdf:resource="&untitled-ontology-7;Domain"/>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#What_is_div? -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;What_is_div?">
        <rdf:type rdf:resource="&untitled-ontology-7;Question"/>
    </owl:NamedIndividual>
    <rdf:Description>
        <rdf:type rdf:resource="&owl;NegativePropertyAssertion"/>
        <owl:targetIndividual rdf:resource="&untitled-ontology-7;HTML"/>
        <owl:sourceIndividual rdf:resource="&untitled-ontology-7;What_is_div?"/>
        <owl:assertionProperty rdf:resource="&untitled-ontology-7;questionBelongToProblem"/>
    </rdf:Description>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#What_is_htmls? -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;What_is_htmls?">
        <rdf:type rdf:resource="&untitled-ontology-7;Question"/>
    </owl:NamedIndividual>
    <rdf:Description>
        <rdf:type rdf:resource="&owl;NegativePropertyAssertion"/>
        <owl:targetIndividual rdf:resource="&untitled-ontology-7;HTML"/>
        <owl:sourceIndividual rdf:resource="&untitled-ontology-7;What_is_htmls?"/>
        <owl:assertionProperty rdf:resource="&untitled-ontology-7;questionBelongToProblem"/>
    </rdf:Description>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#What_is_jQuery? -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;What_is_jQuery?">
        <rdf:type rdf:resource="&untitled-ontology-7;Question"/>
    </owl:NamedIndividual>
    <rdf:Description>
        <rdf:type rdf:resource="&owl;NegativePropertyAssertion"/>
        <owl:targetIndividual rdf:resource="&untitled-ontology-7;JavaScript"/>
        <owl:sourceIndividual rdf:resource="&untitled-ontology-7;What_is_jQuery?"/>
        <owl:assertionProperty rdf:resource="&untitled-ontology-7;questionBelongToProblem"/>
    </rdf:Description>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#kolokvijum_1 -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;kolokvijum_1">
        <rdf:type rdf:resource="&untitled-ontology-7;Test"/>
        <startTimeOfTest rdf:datatype="&xsd;dateTime">21-20-2021</startTimeOfTest>
    </owl:NamedIndividual>
    


    <!-- http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#kolokvijum_2 -->

    <owl:NamedIndividual rdf:about="&untitled-ontology-7;kolokvijum_2">
        <rdf:type rdf:resource="&untitled-ontology-7;Test"/>
        <startTimeOfTest rdf:datatype="&xsd;dateTime">22-02-2022</startTimeOfTest>
    </owl:NamedIndividual>""")


    for line in subjects:
        file.write(line)
    for line in courses:
        file.write(line)

    file.write("</rdf:RDF>")
    file.close()
    driver.close()
